# 🎮 Sparta3D_Adventure

> 유니티 3D 게임 개인 프로젝트
> 3d게임의 여러가지 요소를 구현

---

## 📽️ 데모

> 시연 영상
> https://www.youtube.com/watch?v=HGxcBKxGgKo
> 개발 과정 기록
> [노션 링크](https://www.notion.so/Sparta3D_Adventure-1f9dd79e416180d68c14f155217998b9#1fadd79e416180b291ead3d614af44eb)

---

## 🛠️ 기술 

- **엔진**: Unity (2022.3.17f1)
- **언어**: C#
- **버전관리**: Git & GitHub
---

## 🧩 주요 기능
- 기본 이동 및 점프
- 체력바 UI, 스테미나 UI
- 동적 환경 조사
- 점프대
- 소비아이템 사용 (체력회복, 이속증가, 스테미나회복)
- 3인칭, 1인칭 전환 기능(직접 구현)
- 움직이는 플렛폼 구현 -> dalta활용
- 벽 타기 및 매달리기
- 장비 아이템 구면 및 장착 기능
---

## 🗂️ 폴더 구조 

Scripts/
├── Player/
│   ├── Player.cs               # Player 종합 클래스 (하위 컴포넌트 참조 보관)
│   ├── PlayerController.cs     # 이동, 시점 등 플레이어 조작관련
│   ├── PlayerInteraction.cs    # 상호작용 (아이템 줍기/환경 인터랙션)
│   ├── PlayerCondition.cs      # 플레이어 체력/스태미나 등 상태 관리
│   ├── PlayerEquipment.cs      # 플레이어 장비 장착/해제 및 능력치
│   ├── PlayerInputHandler.cs   # 플레이어 입력 핸들러, PlayerAction.IPlayerActions 상속
│   └── PlayerManager.cs        # 싱글톤 PlayerManager (및 Singleton.cs)
├── Inventory/
│   ├── EInventoryUI.cs         #  인벤토리 총괄 관리 클래스
│   └── QuickSlotUI.cs          #  소비형 아이템 인벤토리 데이터/로직
├── Item/
│   ├── ItemData.cs             # 아이템 ScriptableObject 정의
│   └── ItemObject.cs           # 월드 아이템 오브젝트 (IInteractable 구현)
│
├── Platform/
│   ├── ClimbingPlatform.cs     # 클라이밍 벽 오브젝트 스크립트
│   ├── JumpPlatform.cs         # 점프대 오브젝트 스크립트
│   ├── LaunchPlatform.cs       # 발사대 오브젝트 스크립트
│   └── MovePlatform.cs         # 움직이는 발판 스크립트
│   
├── Global/
│   ├── HealthAction.cs          # 아이템 사용 액션 구현체 : 체력 회복 수치
│   ├── IDamageable.cs           # 피해 인터페이스
│   ├── IInteractable.cs         # 상호작용 인터페이스
│   ├── IItemAction.cs           # 아이템 사용 액션 인터페이스
│   ├── Singleton.cs             # 싱글톤 클래스(Manager에게 상속) 
│   ├── SpeedUpAction.cs         # 아이템 사용 액션 구현체 : 이동속도 증가 수치
│   └── StaminaAction.cs         # 아이템 사용 액션 구현체 : 스테미나 회복 수치
│  
├──etc/
│   ├── CampFire.cs              # 모닥불 오브젝트용 스크립트
│   ├── CampFireHeal.cs          # 모닥불 근처 치유 영역 스크립트
│   └── SceneLoder.cs            # 아무의미없는 스크립트(세션강의 중 급하게 필기한 파일)
└── UI/
    ├── Condition.cs             # 아이템 사용 액션 구현체 : 체력 회복 수치
    ├── DamageIndicater.cs       # 피해 입을 경우 시 출력되는 UI
    ├── UICondition.cs           # 플레이어 체력/스태미나 UI 관리
    ├── Inventory/
    │   ├── EInventoryUI.cs      # 장비 인벤토리 슬롯 UI 관리
    │   ├── QuickSlotUI.cs       # 소비형 인벤토리 슬롯 UI 관리 
    └── Slots/
        ├── EItemSlot.cs         # 장비 인벤토리 슬롯 
        └── QuickSlotItem.cs     # 소비형 인벤토리 슬롯
        
## 🛠️ Self Feedback
- 예외 처리 및 방어적 코드 부족함
- 구조가 커지면서 각 기능이 서로 얽히는 느낌이 생겼고, 특히 PlayerController가 역할이 많아지면서 의존성이 커졌다.
- Unity 컴포넌트(들)에 대한 이해가 부족하다
- 싱글톤 클래스를 따로 만들어서 상속해보았다.
- 추후 주말동안 리펙토링하면서 더 작성해보겠다.


    
