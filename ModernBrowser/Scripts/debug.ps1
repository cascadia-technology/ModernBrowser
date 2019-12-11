param(
	[string]$OutputPath
)

function IsAdministrator
{
    $Identity = [System.Security.Principal.WindowsIdentity]::GetCurrent()
    $Principal = New-Object System.Security.Principal.WindowsPrincipal($Identity)
    $Principal.IsInRole([System.Security.Principal.WindowsBuiltInRole]::Administrator)
}

function IsUacEnabled
{
    (Get-ItemProperty HKLM:\Software\Microsoft\Windows\CurrentVersion\Policies\System).EnableLua -ne 0
}

if (!(IsAdministrator))
{
    if (IsUacEnabled)
    {
        [string[]]$argList = @('-NoProfile', '-File', $MyInvocation.MyCommand.Path)
        $argList += $MyInvocation.BoundParameters.GetEnumerator() | Foreach {"-$($_.Key)", "$($_.Value)"}
        $argList += $MyInvocation.UnboundArguments
        Start-Process PowerShell.exe -Verb Runas -WorkingDirectory $pwd -ArgumentList $argList
        return
    }
    else
    {
        throw "You must be administrator to run this script"
    }
}

try {
	$OutputPath = $OutputPath.Trim()
	foreach ($p in Get-Process -Name Client -ErrorAction Ignore -Verbose) {
		$fi = [IO.FileInfo]$p.Path
		if ($fi.Directory.Name -eq "XProtect Smart Client") {
			$p | Stop-Process -Verbose
		}
	}
	$retries = 0
	while ($null -ne (Get-Process -Name Client -ErrorAction Ignore)) {
		$retries++
		if ($retries -gt 3) {
			throw "Client.exe is still running"
		}
		Write-Warning "Waiting for Smart Client to terminate. . ."
		Start-Sleep -Seconds 1
	}

	$projectName = (Get-Item $PSScriptRoot\..\).BaseName

	$pluginPath = Join-Path "C:\Program Files\VideoOS\MIPPlugins" $projectName
	if (Test-Path $pluginPath) {
		Remove-Item $pluginPath -Recurse -Force
	}
	New-Item $pluginPath -ItemType Directory
	Get-ChildItem $OutputPath | Copy-Item -Destination $pluginPath -Recurse -Container

	$pluginPath = Join-Path "C:\Program Files (x86)\VideoOS\MIPPlugins" $projectName
	if (Test-Path $pluginPath) {
		Remove-Item $pluginPath -Recurse -Force
	}
	New-Item $pluginPath -ItemType Directory
	Get-ChildItem $OutputPath | Copy-Item -Destination $pluginPath -Recurse -Container
}
catch {
	Write-Error $_
	Read-Host
}

