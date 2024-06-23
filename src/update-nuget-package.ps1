param(
[Parameter(Mandatory=$true)]
[string]$projectFilePath,

[Parameter(Mandatory=$true)]
[string]$packageId
)

$pattern = [regex]"((PackageReference)+.(Include=""$packageId"").+(Version=""(.+)\""))"

$content = Get-Content -Path $projectFilePath -Raw
$matches = $pattern.Matches($content)

if ($matches.Count -eq 0)
{
    Write-Host "No $packageId package reference found in the project file."
    return;
}

if ($matches.Count -gt 1)
{
    Write-Host "Multiple $packageId package references found in the project file."
    return;
}

$match = $matches[0]

$version = $match.Groups[5].Value
Write-Host "Current Version`n`n$version`n"

try
{
    $latestVersionResponse = Invoke-RestMethod "https://api.nuget.org/v3-flatcontainer/$($packageId.ToLowerInvariant() )/index.json"
}
catch
{
    Write-Warning "Failed to retrieve the latest version of $packageId."
    return;
}

$index = $latestVersionResponse.versions.IndexOf($version)
if ($index -eq -1)
{
    Write-Host "$version is not found in the list of versions on NuGet."
    return;
}

if ($index -eq $latestVersionResponse.versions.Count - 1)
{
    Write-Host "$packageId is up-to-date."
    return;
}

$updatedVersion = $latestVersionResponse.versions[$index + 1]

Write-Host "Next Version`n`n$updatedVersion`n"

$newContent = $match.Value -replace "(Version=`"(.*?)`")", "Version=""$updatedVersion"" "
Write-Host $newContent

$updatedContent = $content.Replace($match.Value, $newContent)
Write-Verbose $updatedContent

$updatedContent | Set-Content -Path $projectFilePath -NoNewline -Encoding Default

return @{
    InitialVersion = $version
    UpdatedVersion = $updatedVersion
    Updated = $version -ne $updatedVersion
}


