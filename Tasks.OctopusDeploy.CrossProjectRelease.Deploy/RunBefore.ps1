# Set appSettings values

param (
    [string]$fileName
 )

$config = "Tasks.OctopusDeploy.CrossProjectRelease.Deploy.exe.config"
$doc = New-Object System.Xml.XmlDocument
$doc.Load($config)

$doc.SelectSingleNode('configuration/appSettings/add[@key="Tasks.OctopusDeploy.CrossProjectRelease.FileName"]').Attributes['value'].Value = $fileName

$doc.Save($config)
