
function Has-Version {
  param ($version)

  # Check if the version argument is provided
  if ($version.Count -eq 0) {
      Write-Host "Please provide the version number as an argument. Usage: .\bump-version.ps1 <new-version>"
        exit
  }

  # Get the new version from the CLI argument
  return $version[0]
}

$newVersion = Has-Version($args)

# Function to validate the version format (X.X.X where X is a number)
function Validate-VersionFormat {
    param (
        [string]$version
    )

    # Regex pattern for validating version format (X.X.X)
    $versionPattern = '^\d+\.\d+\.\d+$'

    # Check if version matches the pattern
    return $version -match $versionPattern
}

# Function to update version in a file
function Update-Version {
    param (
        [string]$filePath,
        [string]$searchPattern,
        [string]$newVersion,
        [string]$replacementPattern
    )

    # Read the content of the file
    $content = Get-Content $filePath

    # Replace the version based on the provided pattern and replacement
    $updatedContent = $content -replace $searchPattern, $replacementPattern

    # Write the updated content back to the file
    Set-Content $filePath -Value $updatedContent

    Write-Host "Updated value in $filePath. From $searchPattern to $replacementPattern"
}

# Check if the version format is valid
if (-not (Validate-VersionFormat $newVersion)) {
    Write-Host "Invalid version format. Please use the format: X.X.X where X is a number."
    exit
}

$currentYear = (Get-Date).Year

# Define the paths and patterns for each file
$filesToUpdate = @(
    @{
        FilePath = ".\OasysGH\OasysGH.csproj"
        SearchPattern = '<Version>(.*?)<\/Version>'
        ReplacementPattern = "<Version>$newVersion</Version>"
    },
    @{
        FilePath = ".\GH_UnitNumber\GH_UnitNumber.csproj"
        SearchPattern = '<Version>(.*?)<\/Version>'
        ReplacementPattern = "<Version>$newVersion</Version>"
    },
    @{
        FilePath = ".\OasysGH\OasysPluginInfo.cs"
        SearchPattern = 'Version = "(.*?)"'
        ReplacementPattern = 'Version = "' + $newVersion + '"'
    },
    @{
      FilePath = ".\GH_UnitNumber\GH_UnitNumber.csproj"
      SearchPattern = "Oasys \d{4}"
      ReplacementPattern = "Oasys $currentYear"
    },
    @{
        FilePath = ".\GH_UnitNumber\GH_UnitNumberInfo.cs"
        SearchPattern = ' 1985 - \d{4}'
        ReplacementPattern = " 1985 - $currentYear"
    },
    @{
        FilePath = ".\LICENSE"
        SearchPattern = '\d{4} Oasys'
        ReplacementPattern = "$currentYear Oasys"
    },
    @{
        FilePath = ".\GH_UnitNumber\LICENSE"
        SearchPattern = '2020-\d{4} Oasys'
        ReplacementPattern = "2020-$currentYear Oasys"
    },
    @{
        FilePath = ".\OasysGH\LICENSE"
        SearchPattern = '2020-\d{4} Oasys'
        ReplacementPattern = "2020-$currentYear Oasys"
    },
    @{
        FilePath = ".\OasysGH\OasysGH.csproj"
        SearchPattern = 'Oasys \d{4}'
        ReplacementPattern = "Oasys $currentYear"
    },
    @{
        FilePath = ".\OasysGHTestComponents\OasysGHTestComponents.csproj"
        SearchPattern = ' Oasys \d{4}'
        ReplacementPattern = " Oasys $currentYear"
    }
)

# Loop through each file and update the version
foreach ($file in $filesToUpdate) {
    Update-Version -filePath $file.FilePath -searchPattern $file.SearchPattern -newVersion $newVersion -replacementPattern $file.ReplacementPattern
}

Write-Host "Version update completed."