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
        public Action<Fight> OnFightOver;

        private bool _isFightOver;

        private void Start()
        {
            if (StartManager.Instance != null)
            {
                //Subscribe(StartManager.Instance.networkManager);
            }

            fightManager = FightManager.Instance;
            fightManager.OnFightStart += OnFightStart;

            _isFightOver = false;
        }

        /*protected override void Subscribe(NetworkManager networkManager)
        {
            Cancellation = networkManager.Subscribe(this);
        }*/

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
            /*switch (socketMessage.Channel)
            {
                case var value when string.Equals(value,
                    string.Concat(FightChannelSuffix, StartManager.Instance.GetAuthentication().username)):
                    switch (socketMessage.Action)
                    {
                        case AttackAction:
                            //Debug.Log("attack action");
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
            }*/
        }

        public void Attack(FightActionType actionType)
        {
            //Debug.Log("try attack action");
            /*if (!_isFightOver)
            {
                StartManager.Instance.networkManager.SendSocket(new SocketMessage()
                {
                    Action = TryAttack,
                    Channel = string.Concat(FightChannelSuffix, StartManager.Instance.GetAuthentication().username),
                    Username = StartManager.Instance.GetAuthentication().username,
                    Content = actionType.ToString(),
                });
            }*/

            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TryAttack },
                { "channel", string.Concat(SocketChannel.FightChannelSuffix, StartManager.Instance.GetAuthentication().username) },
                { "username", StartManager.Instance.GetAuthentication().username },
                { "content", actionType.ToString() },
            }));*/
        }
        
        private void OnFightStart()
        {
            onFightStart?.Invoke();
        }

        private void OnFightOverAction(string content)
        {
            _isFightOver = true;
            Fight fight = JsonConvert.DeserializeObject<Fight>(content);

            if (fightManager != null)
            {
                fightManager.ActionsComplete += () =>
                {
                    OnFightOver?.Invoke(fight);
                };
            }
            else
            {
                onFightStart = () =>
                {
                    fightManager.ActionsComplete += () =>
                    {
                        OnFightOver?.Invoke(fight);
                    };
                };
            }
        }
        
        protected override void OnDestroy()
        {
            if (Cancellation != null)
            {
                Cancellation.Dispose();
            }
        }
    }
}