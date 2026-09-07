#!/usr/bin/env bash
# Recorrido guiado del sistema actual, en una sola corrida.
#
# Genera dos salidas con la MISMA entrada:
#   salida-demo.txt            configuración que se entrega
#                              (regla estándar, sin convenios)
#   salida-demo-convenios.txt  con el convenio del 10 % encendido
#
# La diferencia entre ambas es la demostración de Strategy sobre
# IDescuento: Ana (cédula 456) paga menos sin que cambie una línea.
set -u

BASE="$(cd "$(dirname "$0")" && pwd)"
REPO="$(cd "$BASE/../.." && pwd)"
APP="$REPO/03-src/SolucionFarmacia/AppFarmaciaConsola"

command -v dotnet >/dev/null 2>&1 || {
    echo "ERROR: dotnet no está instalado en este entorno." >&2; exit 2; }

echo "Compilando..."
(cd "$REPO/03-src/SolucionFarmacia" && dotnet build -v q --nologo) || exit 1

# Localiza el binario sin depender de la versión del framework: el
# proyecto se retargeteó de net8.0 a net10.0 y puede volver a moverse.
localizar_bin() {  # localizar_bin <dir-app>
    ls -d "$1"/bin/Debug/net*/AppFarmaciaConsola 2>/dev/null | head -1
}

BIN="$(localizar_bin "$APP")"
[ -n "$BIN" ] || { echo "ERROR: no hay binario en $APP/bin/Debug/net*/" >&2; exit 1; }

correr() {
    ( cd "$APP" && env -u FARMACIA_REGLA_PUNTOS -u FARMACIA_CONVENIOS \
        "$@" "$BIN" ) < "$BASE/entrada-demo.txt" 2>&1
}

# Normaliza la fecha/hora de los movimientos: si las dos corridas caen a
# distinto minuto, el diff mostraría ruido que no es comportamiento.
normalizar() { sed -E 's/^[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2} - /FECHA - /'; }

correr                      | normalizar > "$BASE/salida-demo.txt"
correr FARMACIA_CONVENIOS=1 | normalizar > "$BASE/salida-demo-convenios.txt"

echo
echo "Generadas: salida-demo.txt y salida-demo-convenios.txt"
echo
echo "Diferencia entre entregar sin convenios y con el convenio del 10 %:"
diff "$BASE/salida-demo.txt" "$BASE/salida-demo-convenios.txt" | sed 's/^/  /'
