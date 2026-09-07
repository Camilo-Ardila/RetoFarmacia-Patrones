#!/usr/bin/env bash
# Casos de caracterización del Reto 2.
#
# Compara el sistema ANTERIOR (estado del repositorio justo antes del
# trabajo de patrones, traído con `git worktree`) contra el sistema
# ACTUAL, en dos grupos con criterios distintos:
#
#   invariantes  -> la salida debe ser IDÉNTICA. Cualquier diferencia
#                   es una regresión.
#   autorizados  -> la salida DEBE diferir, porque la solicitud de
#                   convenios (P-04) autorizó cambiar la pantalla de
#                   venta. Lo que se verifica es que la diferencia sea
#                   exactamente la registrada en diferencias/*.esperado
#                   y no haya crecido.
#
# Uso:  ./correr-casos.sh            corre y verifica
#       ./correr-casos.sh --fijar    además congela las diferencias
#                                    autorizadas como referencia
#       ./correr-casos.sh --limpiar  borra el worktree de la línea base
set -u

BASE="$(cd "$(dirname "$0")" && pwd)"
REPO="$(cd "$BASE/../.." && pwd)"
BASELINE="${FARMACIA_BASELINE:-ffaf533}"
WT="${TMPDIR:-/tmp}/farmacia-anterior-$BASELINE"

ACTUAL="$REPO/03-src/SolucionFarmacia"
ANTERIOR="$WT/03-src/SolucionFarmacia"

if [ "${1:-}" = "--limpiar" ]; then
    git -C "$REPO" worktree remove --force "$WT" 2>/dev/null
    rm -rf "$WT"
    echo "Worktree de la línea base eliminado."
    exit 0
fi

FIJAR=0
[ "${1:-}" = "--fijar" ] && FIJAR=1

if ! command -v dotnet >/dev/null 2>&1; then
    echo "ERROR: dotnet no está instalado en este entorno." >&2
    echo "       Este script necesita compilar los dos sistemas." >&2
    exit 2
fi

# --- Línea base: el repositorio en el commit anterior a los patrones ---
if [ ! -d "$ANTERIOR" ]; then
    echo "Creando worktree de la línea base ($BASELINE)..."
    git -C "$REPO" worktree add --detach "$WT" "$BASELINE" || exit 1
fi

# La línea base es un commit anterior al cambio de framework, así que sus
# proyectos piden net8.0. El worktree es desechable: se le ajusta el
# framework para que corra con el SDK instalado. No toca el repositorio.
find "$WT" -name '*.csproj' -exec \
    sed -i 's|<TargetFramework>net8\.0</TargetFramework>|<TargetFramework>net10.0</TargetFramework>|' {} +

echo "Compilando sistema anterior ($BASELINE)..."
(cd "$ANTERIOR" && dotnet build -v q --nologo) || exit 1
echo "Compilando sistema actual..."
(cd "$ACTUAL" && dotnet build -v q --nologo) || exit 1
echo

# Normaliza lo que varía entre corridas y no es comportamiento:
#   - la fecha/hora de los movimientos (DateTime.Now)
#   - las rutas absolutas y las líneas de traza de una excepción
normalizar() {
    sed -E \
        -e 's/^[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2} - /FECHA - /' \
        -e 's#(/[^ ]*/)(BibFarmacia|AppFarmaciaConsola)#RUTA/\2#g' \
        -e 's/^ +(at|en) .*$/   [traza]/' \
        "$1"
}

# Localiza el binario sin depender de la versión del framework: el
# proyecto se retargeteó de net8.0 a net10.0 y puede volver a moverse.
localizar_bin() {  # localizar_bin <dir-app>
    ls -d "$1"/bin/Debug/net*/AppFarmaciaConsola 2>/dev/null | head -1
}

correr() {  # correr <dir-app> <archivo-entrada> <archivo-salida>
    local bin; bin="$(localizar_bin "$1")"
    [ -n "$bin" ] || { echo "ERROR: no hay binario en $1/bin/Debug/net*/" >&2; exit 1; }
    # El sistema anterior aborta a propósito en a03 (excepción no
    # controlada al vender más de lo que hay). Eso es la evidencia, no un
    # fallo del script, así que se ejecuta en un shell aparte cuyo aviso
    # de "Aborted (core dumped)" se descarta. La salida y la traza de la
    # aplicación sí quedan en el archivo.
    bash -c 'cd "$1" && env -u FARMACIA_REGLA_PUNTOS -u FARMACIA_CONVENIOS \
        "$2" < "$3" > "$4" 2>&1' _ "$1" "$bin" "$2" "$3.raw" 2>/dev/null
    normalizar "$3.raw" > "$3"
    rm -f "$3.raw"
}

fallos=0
avisos=0

echo "--- Invariantes (deben coincidir) ---"
for entrada in "$BASE"/entradas-invariantes/c*.txt; do
    caso="$(basename "$entrada" .txt)"
    correr "$ANTERIOR/AppFarmaciaConsola" "$entrada" "$BASE/salidas-anterior/$caso.txt"
    correr "$ACTUAL/AppFarmaciaConsola"   "$entrada" "$BASE/salidas-actual/$caso.txt"

    if diff -q "$BASE/salidas-anterior/$caso.txt" \
               "$BASE/salidas-actual/$caso.txt" >/dev/null; then
        echo "OK       $caso"
    else
        echo "REGRESIÓN $caso"
        diff "$BASE/salidas-anterior/$caso.txt" \
             "$BASE/salidas-actual/$caso.txt" | head -20
        fallos=$((fallos + 1))
    fi
done

echo
echo "--- Cambio autorizado (deben diferir, y solo en lo declarado) ---"
for entrada in "$BASE"/entradas-autorizados/anterior/a*.txt; do
    caso="$(basename "$entrada" .txt)"
    correr "$ANTERIOR/AppFarmaciaConsola" \
           "$BASE/entradas-autorizados/anterior/$caso.txt" \
           "$BASE/salidas-anterior/$caso.txt"
    correr "$ACTUAL/AppFarmaciaConsola" \
           "$BASE/entradas-autorizados/actual/$caso.txt" \
           "$BASE/salidas-actual/$caso.txt"

    diff -u "$BASE/salidas-anterior/$caso.txt" \
            "$BASE/salidas-actual/$caso.txt" \
        | tail -n +3 > "$BASE/diferencias/$caso.diff"

    if [ ! -s "$BASE/diferencias/$caso.diff" ]; then
        echo "SOSPECHOSO $caso — no hay diferencia y debería haberla"
        avisos=$((avisos + 1))
        continue
    fi

    ref="$BASE/diferencias/$caso.esperado"
    if [ "$FIJAR" -eq 1 ]; then
        cp "$BASE/diferencias/$caso.diff" "$ref"
        echo "FIJADO   $caso ($(grep -c '^[+-]' "$ref") líneas de diferencia)"
    elif [ ! -f "$ref" ]; then
        echo "SIN REF  $caso — corra una vez con --fijar"
        avisos=$((avisos + 1))
    elif diff -q "$ref" "$BASE/diferencias/$caso.diff" >/dev/null; then
        echo "OK       $caso (diferencia contenida)"
    else
        echo "CRECIÓ   $caso — la diferencia ya no es la autorizada"
        diff "$ref" "$BASE/diferencias/$caso.diff" | head -20
        fallos=$((fallos + 1))
    fi
done

echo
if [ "$fallos" -eq 0 ] && [ "$avisos" -eq 0 ]; then
    echo "RESULTADO: sin regresiones; el cambio autorizado está contenido."
else
    echo "RESULTADO: $fallos fallo(s), $avisos aviso(s)."
fi
exit "$fallos"
