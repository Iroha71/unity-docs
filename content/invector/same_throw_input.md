# エイムと投擲を同じボタンで行う

- **vThrowManagerBase** > **UpdateThrowInput()** を編集する

  ``` csharp
  protected virtual void UpdateThrowInput()
  {
      if (!ThrowConditions)
      {
          return;
      }

      if (aimThrowInput.GetButtonDown() 
        && !inEnterThrowMode 
        && !isThrowing 
        && !isAiming)
      {
          EnterThrowMode();
          return;
      }
      // ↓エイム解除処理をコメントアウト
      //if (aimThrowInput.GetButtonUp() && aimHoldingButton && (isAiming || inEnterThrowMode) && !isThrowing)
      //{
      //    ExitThrowMode();
      //}

      if (isAiming 
        && !isThrowing 
        && !pressThrowInput)
      {
          // throwInput→aimThrowInputへ変更
          if (aimThrowInput.GetButtonDown())
              pressThrowInput = true;
      }

      if (!aimHoldingButton 
        && aimThrowInput.GetButtonDown() 
        && !pressThrowInput 
        && (isAiming || inEnterThrowMode) 
        && !isThrowing)
      {
          ExitThrowMode();
      }
  }
  ```