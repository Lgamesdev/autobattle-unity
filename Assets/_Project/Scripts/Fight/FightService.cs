using System;
using System.Collections.Generic;
using LGamesDev.Core;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace LGamesDev.Fighting
{
    public class FightService : BaseNetworkService
    {
        //Socket Send Actions
        private const string TryAttack = "tryAttack";
        
        //Socket Receive Actions
        private const string AttackAction = "attack";
        private const string ExperienceGained = "experienceGained";
        private const string FightOver = "fightOver";
        
        protected override IDisposable Cancellation { get; set; }
        
        private static FightManager fightManager;

        private static Action onFightStart;
        public static Action OnFightOver;

        private void Start()
        {
            Subscribe(GameManager.Instance.networkManager);
            fightManager = FightManager.Instance;
            fightManager.OnFightStart += OnFightStart;
        }

        protected override void Subscribe(NetworkManager networkManager)
        {
            Cancellation = networkManager.Subscribe(this);
        }

        public override void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public override void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public override void OnNext(SocketMessage socketMessage)
        {
            HandleSocket(socketMessage);
        }

        protected override void HandleSocket(SocketMessage socketMessage)
        {
            switch (socketMessage.Channel)
            {
                case var value when string.Equals(value,
                    string.Concat(FightChannelSuffix, GameManager.Instance.GetAuthentication().username)):
                    switch (socketMessage.Action)
                    {
                        case AttackAction:
                            List<FightAction> fightActions =
                                JsonConvert.DeserializeObject<List<FightAction>>(socketMessage.Content);
                            FightManager.Instance.AddActions(fightActions);
                            break;
                        case ExperienceGained:
                            Dictionary<string, int> experiencePayload =
                                JsonConvert.DeserializeObject<Dictionary<string, int>>(socketMessage.Content);
                            FightManager.Instance.playerCharacterFight.GetLevelSystem().AddExperience(
                                experiencePayload["level"],
                                experiencePayload["oldExperience"],
                                experiencePayload["aimedExperience"],
                                experiencePayload["maxExperience"]
                            );
                            break;
                        case FightOver:
                            OnFightOverAction(socketMessage.Content);
                            break;
                    }

                    break;
            }
        }

        public void Attack(FightActionType actionType)
        {
            GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TryAttack,
                Channel = string.Concat(FightChannelSuffix, GameManager.Instance.GetAuthentication().username),
                Username = GameManager.Instance.GetAuthentication().username,
                Content = actionType.ToString(),
            });
            
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TryAttack },
                { "channel", string.Concat(SocketChannel.FightChannelSuffix, GameManager.Instance.GetAuthentication().username) },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", actionType.ToString() },
            }));*/
        }
        
        private void OnFightStart()
        {
            onFightStart?.Invoke();
        }

        private static void OnFightOverAction(string content)
        {
            Fight fight = JsonConvert.DeserializeObject<Fight>(content);
            
            if (fightManager != null)
            {
                fightManager.Fight.PlayerWin = fight.PlayerWin;
                fightManager.Fight.Reward = fight.Reward;
                fightManager.ActionsComplete += () =>
                {
                    OnFightOver?.Invoke();
                };
            }
            else
            {
                onFightStart = () =>
                {
                    fightManager.Fight.PlayerWin = fight.PlayerWin;
                    fightManager.Fight.Reward = fight.Reward;
                    fightManager.ActionsComplete += () =>
                    {
                        OnFightOver?.Invoke();
                    };
                };
            }
        }
    }
}