# ロックオンの強制解除

- **vLockOn**に以下を追記

    ``` csharp
    public void UnLockForce()
    {
        isLockingOn = false;
        LockOn(false);
    }
    ```
