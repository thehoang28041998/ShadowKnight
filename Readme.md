Logic:
- WorldManager
- System: 
  + UserInputSystem	: Chỉ sử dụng cho Main Entity
  + StateMachineSystem 	: lắng nghe, xử lý việc chuyển trạng thái
    + IdleStateJobSystem: Mỗi 1 JobSystem sẽ xử lý 1 JobComponent (tương đương 1 State)
    + RunStateJobSystem :
  + RequestSystem	: lắng nghe, xử lý queue request
    + RunJobSystem      : Xử lý JobComponent (tương đương Request)
    + DashJobSystem
  + TranslateSystem	: Di chuyển dựa trên các component đăng kí

- Component
  + InputComponent
  + TranslateComponent
  + RequestComponent
  + VelocityComponent
  + RunComponent
  + DashComponent
  + AnimationComponent
  + IdleStateComponent
  + RunStateComponent
  + DashStateComponent
  + AttackStateComponent
  + StateMachineComponent