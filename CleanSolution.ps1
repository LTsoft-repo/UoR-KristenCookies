$foldersToRemove = 'bin','obj'
[string[]]$foldersToIgnore = 'node_module'

Get-ChildItem .\ -Include $foldersToRemove -Recurse | Where-Object { $_.FullName -inotmatch "\\$($foldersToIgnore -join '|')\\" } | Remove-Item -Force -Recurse
