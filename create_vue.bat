@echo off
setlocal enabledelayedexpansion

REM content/invector配下の.mdを取得
set "source_dir=content\invector"
set "target_dir=pages\invector"

for %%F in (%source_dir%\*.md) do (
  set "filename=%%~nF"
  set "vue_file=%target_dir%\!filename!.vue"
  if not exist "!vue_file!" (
    echo 作成中: "!vue_file!"

    (
      echo ^<template^>
      echo   ^<main^>
      echo     ^<ContentDoc/^>
      echo   ^</main^>
      echo ^</template^>
    ) > "!vue_file!"
  ) else (
    echo スキップ: "!vue_file!"
  )
)

pause