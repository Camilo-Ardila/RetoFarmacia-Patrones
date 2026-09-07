#!/usr/bin/env bash
# Demostración ejecutable de los patrones sobre el sistema ACTUAL.
#
# La prueba de que Strategy está bien aplicado no es que el código
# compile: es que la MISMA entrada produzca salidas distintas cambiando
# solo la configuración del punto de ensamblaje, sin editar una línea
# ni recompilar. Por eso las estrategias vigentes se eligen con variable
# de entorno y no editando Program.cs entre corridas.
#
# Uso:  ./correr-patrones.sh
set -u

BASE="$(cd "$(dirname "$0")" && pwd)"
REPO="$(cd "$BASE/../.." && pwd)"
APP="$REPO/03-src/SolucionFarmacia/AppFarmaciaConsola"
OUT="$BASE/salidas"

command -v dotnet >/dev/null 2>&1 || {
    echo "ERROR: dotnet no está instalado en este entorno." >&2; exit 2; }

mkdir -p "$OUT"
echo "Compilando..."
(cd "$REPO/03-src/SolucionFarmacia" && dotnet build -v q --nologo) || exit 1
echo

# Localiza el binario sin depender de la versión del framework: el
# proyecto se retargeteó de net8.0 a net10.0 y puede volver a moverse.
localizar_bin() {  # localizar_bin <dir-app>
    ls -d "$1"/bin/Debug/net*/AppFarmaciaConsola 2>/dev/null | head -1
}

BIN="$(localizar_bin "$APP")"
[ -n "$BIN" ] || { echo "ERROR: no hay binario en $APP/bin/Debug/net*/" >&2; exit 1; }

correr() {  # correr <etiqueta-salida> <entrada> [VAR=valor ...]
    local salida="$OUT/$1.txt"; local entrada="$2"; shift 2
    ( cd "$APP" && env -u FARMACIA_REGLA_PUNTOS -u FARMACIA_CONVENIOS \
        "$@" "$BIN" ) < "$entrada" > "$salida" 2>&1
    sed -i -E 's/^[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2} - /FECHA - /' "$salida"
}

comparar() {  # comparar <titulo> <salida-a> <salida-b>
    echo "=== $1 ==="
    if diff -q "$OUT/$2.txt" "$OUT/$3.txt" >/dev/null; then
        echo "FALLA: las dos corridas son idénticas; la estrategia no se aplicó."
        return 1
    fi
    diff "$OUT/$2.txt" "$OUT/$3.txt" | grep -E '^[<>]' | sed 's/^/  /'
    echo
    return 0
}

fallos=0

# --- Strategy: la regla de puntos ---
correr p01-estandar "$BASE/entradas/p01-regla-puntos.txt" FARMACIA_REGLA_PUNTOS=estandar
correr p01-doble    "$BASE/entradas/p01-regla-puntos.txt" FARMACIA_REGLA_PUNTOS=doble
comparar "Strategy — IReglaPuntos (misma entrada, dos reglas)" p01-estandar p01-doble \
    || fallos=$((fallos + 1))

# --- Strategy: el descuento por convenio ---
correr p02-sin-convenio "$BASE/entradas/p02-convenio-descuento.txt"
correr p02-con-convenio "$BASE/entradas/p02-convenio-descuento.txt" FARMACIA_CONVENIOS=1
comparar "Strategy — IDescuento (Ana, cédula 456, convenio del 10 %)" \
    p02-sin-convenio p02-con-convenio || fallos=$((fallos + 1))

# --- Simple Factory + registros de estrategias: capturas ---
correr p03-registro-rellenos "$BASE/entradas/p03-registro-rellenos.txt"
correr p04-estados-venta     "$BASE/entradas/p04-estados-venta.txt"
echo "=== Factory + registros y State: salidas capturadas en salidas/ ==="
echo "  p03-registro-rellenos.txt  — dos cápsulas creadas por la misma fábrica, un relleno cada una"
echo "  p04-estados-venta.txt      — venta que termina en Fallida y venta sin cliente"
echo

if [ "$fallos" -eq 0 ]; then
    echo "RESULTADO: las estrategias son intercambiables sin tocar el código."
else
    echo "RESULTADO: $fallos comprobación(es) fallida(s)."
fi
exit "$fallos"
