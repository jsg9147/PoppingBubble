# Popping Bubble

물리 엔진 기반의 전략적 퍼즐 게임

## 프로젝트 정보

| 항목 | 내용 |
|------|------|
| 개발 기간 | 2024.02 ~ 2024.06 (5개월) |
| 개발 인원 | 1인 개발 |
| 장르 | 물리 기반 퍼즐 게임 |
| 플랫폼 | PC, Android |
| 엔진 / 언어 | Unity 2022, C# |

## 게임 소개

색깔 공을 떨어뜨려 **같은 색 공들이 좌측 벽과 우측 벽을 동시에 연결**하면 제거되는 퍼즐 게임입니다.

- 공은 물리 엔진에 의해 튕기고 굴러가므로 정확한 위치 조절이 어려움
- 연속으로 같은 색을 제거하면 콤보 배율 적용 (×1 → ×10 → ×100)
- 공이 상단까지 쌓이면 게임 오버

## 핵심 기술 구현

### 1. 색상 그룹 연결 탐지 알고리즘

Collider2D 충돌 기반으로 인접한 같은 색 공을 탐지하고, 재귀적 그래프 탐색으로 연결된 모든 공을 찾아냅니다.

```csharp
// Ball.cs - 연결된 모든 같은 색 공 탐색
void RecursiveFindConnectedBalls(Ball current, List<Ball> result, List<Ball> visited)
{
    if (visited.Contains(current)) return;
    visited.Add(current);
    result.Add(current);

    foreach (Ball connected in current.connectedBalls)
    {
        if (connected.colorIndex == this.colorIndex)
            RecursiveFindConnectedBalls(connected, result, visited);
    }
}
```

### 2. 양쪽 벽 연결 판정

각 공이 HashSet으로 접촉 중인 벽을 추적하고, 연결된 그룹 내에서 좌/우 벽 접촉 여부를 검사하여 제거 조건을 판단합니다.

```csharp
// 제거 조건: 그룹 내 공 중 하나라도 좌벽 접촉 AND 하나라도 우벽 접촉
bool TryRemoveIfBothWalls(List<Ball> group)
{
    bool touchLeft = group.Any(b => b.touchedWalls.Contains("LeftWall"));
    bool touchRight = group.Any(b => b.touchedWalls.Contains("RightWall"));
    return touchLeft && touchRight;
}
```

### 3. 콤보 점수 시스템

연속 제거 시 지수 배율을 적용하여 전략적 플레이를 유도합니다.

```csharp
// GameManager.cs
public void AddScore(int baseScore)
{
    int multiplier = (int)Mathf.Pow(10, combo - 1);
    score += baseScore * multiplier;
}
```

### 4. 물리 기반 낙하 시스템

Rigidbody2D와 Physics Material 2D를 활용하여 공의 탄성과 마찰을 조절, 예측하기 어려운 움직임을 구현했습니다.

## 시스템 아키텍처

```
┌─────────────────┐
│  BallController │ ← 공 생성, 드롭 타이밍, 프리뷰 관리
└────────┬────────┘
         │ Instantiate
         ▼
┌─────────────────┐
│      Ball       │ ← 물리 시뮬레이션, 충돌 감지, 그룹 탐색
└────────┬────────┘
         │ RemoveConnectedBalls()
         ▼
┌─────────────────┐
│   GameManager   │ ← 점수 계산, 콤보 관리, 게임 상태
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│    UIManager    │ ← 점수/콤보/게임오버 UI 표시
└─────────────────┘
```

## 프로젝트 구조

```
Assets/
├── 01_Scripts/
│   ├── BaseCode/          # GameManager, ResolutionManager
│   ├── GameContorol/      # Ball, BallController, WaitingBall, EraseMode
│   ├── UI/                # UIManager, ScoreUI, GameoverUI
│   ├── Setting/           # 오디오 설정
│   └── Admob/             # 광고 연동
├── 02_Sprites/            # 공 텍스처, UI 에셋
├── 03_Prefabs/            # 프리팹
├── 05_Sound/              # BGM, SFX
└── 06_Physics Material 2D/ # 물리 머티리얼
```

## 사용 기술

| 분류 | 기술 |
|------|------|
| 게임 엔진 | Unity 2022 |
| 언어 | C# |
| 물리 | Rigidbody2D, Physics Material 2D, Collider2D |
| 오디오 | MasterAudio |
| 애니메이션 | DOTween |
| UI | TextMesh Pro, Unity UI |
| 광고 | Google Mobile Ads |
| 백엔드 | Firebase Authentication, Realtime Database |
| 랭킹 | Google Play Games Services |

## 개발 과정에서의 문제 해결

### 문제 1: 연결 그룹 탐색 시 무한 루프
- **원인**: 양방향 연결된 공들 사이에서 재귀 호출이 무한 반복
- **해결**: visited 리스트로 이미 방문한 공을 추적하여 중복 탐색 방지

### 문제 2: 물리 반동과 게임성 밸런스
- **원인**: 반동이 너무 크면 조작이 불가능, 너무 작으면 전략성 감소
- **해결**: Physics Material 2D의 Bounciness/Friction 값을 반복 테스트하여 최적값 도출

### 문제 3: 제거된 공의 충돌 재감지
- **원인**: 제거 처리 중인 공이 다른 공의 연결 리스트에 남아있어 오류 발생
- **해결**: `isRemoved` 플래그를 도입하여 제거된 공을 연결 탐색에서 제외

## 영상

[![Popping Bubble Trailer](https://img.youtube.com/vi/Y5agvIbqGHk/0.jpg)](https://youtu.be/Y5agvIbqGHk)

물리 기반 낙하, 연결 탐지 시스템, 연쇄 콤보 장면을 확인할 수 있습니다.

## 회고

- 특수 구슬, 추가 패턴 등을 시도했으나 난이도가 과도하게 상승하여 단순화 방향으로 전환
- 물리 반동과 콤보 시스템 간 밸런스 조정에 가장 많은 시간 소요
- UI 피드백 속도와 애니메이션 개선을 통해 더 직관적인 플레이가 가능할 것으로 판단
