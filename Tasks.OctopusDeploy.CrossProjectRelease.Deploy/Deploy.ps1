$Executable = "Tasks.OctopusDeploy.CrossProjectRelease.Deploy.exe"

Write-Verbose "Running $ReturnFromEXE = Start-Process -FilePath $Executable -NoNewWindow -Wait -Passthru"
$ReturnFromEXE = Start-Process -FilePath $Executable -NoNewWindow -Wait -Passthru

Write-Verbose "Returncode is $($ReturnFromEXE.ExitCode)"
if (!($ReturnFromEXE.ExitCode -eq 0)) {
	throw "$Executable failed with exit code $($ReturnFromEXE.ExitCode)"
}
