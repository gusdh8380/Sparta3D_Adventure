# 🎮 Sparta3D_Adventure

- 유니티 3D 게임 개인 프로젝트
- 3d게임의 여러가지 요소를 구현

---

## 📽️ 데모

- 시연 영상
- https://www.youtube.com/watch?v=HGxcBKxGgKo

- 개발 과정 기록
- [노션 링크](https://www.notion.so/Sparta3D_Adventure-1f9dd79e416180d68c14f155217998b9#1fadd79e416180b291ead3d614af44eb)

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

 ## 📁 폴더 구조 보기
```plaintext
Scripts/
├── Player/
│   ├── Player.cs               # Player 종합 클래스
│   ├── PlayerController.cs     # 이동, 시점 등 조작 관련
│   ├── PlayerInteraction.cs    # 아이템/환경 상호작용
│   ├── PlayerCondition.cs      # 체력/스태미나 상태 관리
│   ├── PlayerEquipment.cs      # 장비 장착/해제 및 능력치
│   ├── PlayerInputHandler.cs   # 입력 핸들러
│   └── PlayerManager.cs        # 싱글톤 플레이어 관리자
│
├── Inventory/
│   ├── InventoryUI.cs          # 인벤토리 UI 통합
│   └── QuickSlotUI.cs          # 소비형 인벤토리 UI
│
├── Item/
│   ├── ItemData.cs             # 아이템 데이터(ScriptableObject)
│   └── ItemObject.cs           # 월드 아이템 오브젝트
│
├── Platform/
│   ├── ClimbingPlatform.cs     # 클라이밍 플랫폼
│   ├── JumpPlatform.cs         # 점프대
│   ├── LaunchPlatform.cs       # 발사대
│   └── MovePlatform.cs         # 움직이는 발판
│
├── Global/
│   ├── IDamageable.cs          # 피해 인터페이스
│   ├── IInteractable.cs        # 상호작용 인터페이스
│   ├── ItemAction.cs           # 아이템 액션 인터페이스
│   └── Singleton.cs            # 공용 싱글톤 클래스
│
├── Actions/
│   ├── HealthAction.cs         # 체력 회복
│   ├── StaminaAction.cs        # 스태미나 회복
│   └── SpeedUpAction.cs        # 이동속도 증가
│
├── etc/
│   ├── CampFire.cs             # 모닥불 오브젝트
│   ├── CampFireHeal.cs         # 회복 영역 스크립트
│   └── SceneLoader.cs          # 씬 로딩 유틸(미사용, 세션 강의 필기용)
│
├── UI/
│   ├── Condition.cs            # 체력/스태미나 UI
│   └── UICondition.cs          # UI 연동 클래스
│
└── Slots/
    ├── ItemSlot.cs             # 장비 슬롯
    └── QuickSlotItem.cs        # 소비형 아이템 슬롯
```



## 🛠️ Self Feedback
- 예외 처리 및 방어적 코드 부족함
- 구조가 커지면서 각 기능이 서로 얽히는 느낌이 생겼고, 특히 PlayerController가 역할이 많아지면서 의존성이 커졌다.
- 인벤토리, 아이템 사용, 아이템 장착 코드가 동작은하지만 설계적으로 문제가 있어보인다.
- Unity 컴포넌트(들)에 대한 이해가 부족하다


## 🛠️ Feedback Analysis Summary
1. 소프트웨어 아키텍처 설계 능력 : 명확하고 깔끔한 클래스 분리르 시도하였지만, 역할이 명확이 분리된 아키텍처를 완성하지 못하였다.
2. 이벤트 기반 시스템 구성 능력 : 시스템 전반에 일관적인 적용하는 능력을 키워야 할 것 같다.
3. 데이터 - UI 분리 및 데이터 중심 설계 역량 : 현재 UI 오브젝트가 곧 데이터 저장소 역함을 겸하고 있음, 이를 분리하여 확장성을 확보할 필요가 있어보임
4. 컴포넌트 통신과 의존성 관리 : FindObjectOfType, Singleton의 의존성을 줄이고 컴포넌트 참조를 명시적으로 관리해야

    
