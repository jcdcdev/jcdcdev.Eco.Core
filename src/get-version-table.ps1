$packageId = "jcdcdev.eco.core"
$nugetApiUrl = "https://api.nuget.org/v3/registration5-gz-semver2/$packageId/index.json"

$response = Invoke-RestMethod -Uri $nugetApiUrl

function Sanitize-VersionRange
{
    param (
        [string]$range
    )
    return ($range -replace "[\[\]\(\)]", "").Split(',')[0]
}
function To-GameVer
{
    param (
        [string]$version
    )
    $ver = $version -replace "-.*" -replace "[^0-9.]"
    $ver = $ver.Split('.')
    $output = "$( $ver[1] ).$( $ver[2] )"
    if ($ver[3])
    {
        $output += ".$( $ver[3] )"
    }
    return $output
}

function Check-Dependencies
{
    param (
        [string]$catalogEntryUrl
    )

    $catalogEntry = Invoke-RestMethod -Uri $catalogEntryUrl
    $dependencies = $catalogEntry.dependencyGroups.dependencies

    $ecoReferenceAssemblies = $dependencies | Where-Object { $_.id -eq "Eco.ReferenceAssemblies" }
    if ($ecoReferenceAssemblies)
    {
        return Sanitize-VersionRange -range $ecoReferenceAssemblies.range
    }

    $elixrModsFramework = $dependencies | Where-Object { $_.id -eq "ElixrMods.Framework" }
    if ($elixrModsFramework)
    {
        $cleanRange = Sanitize-VersionRange -range $elixrModsFramework.range

        $elixrModsFrameworkUrl = "https://api.nuget.org/v3/registration5-gz-semver2/elixrmods.framework/$cleanRange.json"
        $elixrModsFrameworkResponse = Invoke-RestMethod -Uri $elixrModsFrameworkUrl
        $elixrCatalogEntryUrl = $elixrModsFrameworkResponse.catalogEntry

        $elixrModsFrameworkCatalogResponse = Invoke-RestMethod -Uri $elixrCatalogEntryUrl

        $ecoReferenceAssembliesInFramework = $elixrModsFrameworkCatalogResponse.dependencyGroups.dependencies | Where-Object { $_.id -eq "Eco.ReferenceAssemblies" }
        if ($ecoReferenceAssembliesInFramework)
        {
            return Sanitize-VersionRange -range $ecoReferenceAssembliesInFramework.range
        }
        else
        {
            return "N/A"
        }
    }
    else
    {
        return "N/A"
    }
}

$results = foreach ($version in $response.items[0].items)
{
    $versionNumber = $version.catalogEntry.version
    $catalogEntryUrl = $version.catalogEntry.PSObject.Properties["@id"].Value

    [PSCustomObject]@{
        Version = $versionNumber
        Result = Check-Dependencies -catalogEntryUrl $catalogEntryUrl
    }
}

$results = $results | Sort-Object -Property Version -Descending

$markdownTable = @"
| Version | Game Version | Full Version |
|---|---|---|`n
"@

$results | ForEach-Object {
    $markdownTable += "| [$( $_.Version )](https://github.com/jcdcdev/jcdcdev.Eco.Core/releases/tag/$( $_.Version )) | $( To-GameVer $_.Result ) | $( $_.Result ) |
"
}

Write-Output $markdownTable