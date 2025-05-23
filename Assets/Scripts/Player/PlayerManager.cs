using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//싱글톤 클래스를 상속해서 만들어본 플레이어 전용 관리자
public class PlayerManager : Singleton<PlayerManager>
{
   
    public Player player;
    public Player Player { get { return player; } set { player = value; } }
    protected override void Awake() => base.Awake();
}
