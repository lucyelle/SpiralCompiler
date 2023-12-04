dotnet pack ../SpiralLangserver --output .
if ((dotnet tool list --global) -match "SpiralLangserver") {
    dotnet tool uninstall --global SpiralLangserver
}
dotnet tool install --global --add-source . SpiralLangserver
