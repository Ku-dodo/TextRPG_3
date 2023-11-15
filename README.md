# TextRPG
### 프로젝트 소개
C# 을 사용해서 제작한 RPG 게임 입니다.
***
### 개발 기간
- 23.11.08 ~ 
***
### 구현 클래스 간단 소개
- Scene Class : 모든 장면의 부모 클래스로 Scene 에 필요한 기본적인 기능들을 가집니다.
  - 씬의 이름, 씬에 대한 코멘트
  - 이전 씬 및 다음으로 이동할 수 있는 씬, 이동할 수 있는 Scene 의 경우 Dictionary 로 관리
  - 화면의 상단 및 하단에 출력할 값 및 출력 함수
  - 안내 메세지 출력

- BaseDungeonScene : 던전 Scene 에 기본적으로 필요한 부분을 추가한 클래스 입니다.
  - 던전에 대한 정보
  - 던전을 진행하는 과정(함수)를 실행
  - 던전 진행과 관련해서 출력할 메세지

- Dungeon : 던전 인스턴스를 생성할 때 사용하는 클래스 입니다.
  - 진행 상태( 진행중, 완료, 실패 )
  - 난이도
  - 이름
  - 보상 ( 골드, 경험치 )
  - 클리어 확률, 진행 시도 횟수
  - 요구 방어력

- Player : 플레이어 인스턴스를 생성할 때 사용하는 클래스 입니다.
  - 능력치( HP, ATK, DEF, EXP )
  - 골드
  - 인벤토리
  - 장비 관리자
 
- EquipManager : 플레이어가 착용한 장비를 관리하는 클래스 입니다.
  - 플레이어가 착용할 수 있는 부위 ( enum )
  - 플레이어가 착용한 장비
  - 장비 착용 , 동일한 부위에 착용하고 있는 장비가 있다면 교체
 
-  Widget : 기본적인 사각 틀을 그리는 클래스 입니다.
   - 사각 틀의 너비 및 높이
   - Draw( x position, y position )

- TextBlock : Widget 내에 text 를 삽입할 수 있는 클래스 입니다.

- ItemSlot : 아이템 정보를 화면에 출력할 때 사용하는 클래스 입니다.
  - 아이템 정보를 출력하는데 필요한 형식을 가집니다.

- Status : 플레이어의 능력치 정보를 보여주는 클래스 입니다.
***
### 구현 기능
- Scene 전환
  - 구분되는 장면을 모두 Scene 클래스를 상속받는 클래스로 생성
  - 전환되는 Scene 의 DrawScene 함수를 호출해서 해당 Scene 에 맞는 화면을 출력

- 아이템 구입 및 판매
  - 플레이어의 Buy 및 Sell 함수를 통해서 구현
    
- 아이템 장착 및 해제
  - 장비 관리자를 통해서 구현
    
- 인벤토리 정렬
  - List 의 Sort(Comparison) 을 통해서 구현
    
- 던전 진행
  - while 문 내에 Thread.Sleep 을 통해서 점점 진행되는 것 처럼 표현

***
### 데이터 저장 및 불러오기
- 배경 및 이미지 : .txt
- 플레이어 및 착용한 장비 : .json
