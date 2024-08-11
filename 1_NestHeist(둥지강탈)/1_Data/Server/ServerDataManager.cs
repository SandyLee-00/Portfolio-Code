using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class ServerDataManager
{
    public ApiService API { get; private set; }
    private RequestQueueManager _queueManager = new RequestQueueManager();


    public ServerUserGameInfoData User;
    public List<ServerUserMonsterData> UserMonsterDataList;
    public List<ServerUserEggData> UserEggDataList;
    public List<ServerUserInventoryData> UserInventoryList;
    public List<ServerUserDungeonProgressData> UserDungeonProgressList;
    public List<ServerIncubatorData> UserIncubatorList;
    public ServerUserFatigueData UserFatigueData;
    public List<ServerCompletedQuestData> CompletedQuestDataList;
    public List<ServerSubtaskData> SubtaskList;

    private bool _isAlreadyInit = false;

    public event Action<UpdatePartnerMonsterBody> OnUpdatePartnerMonster;
    public event Action<UserMonsterLevelUpBody> OnUserMonsterLevelUp;
    public event Action<ServerIncubatorData> OnStartHatching;
    public event Action<ServerUserMonsterData> OnHatchEgg;
    public event Action<ServerUserDungeonProgressData> OnUpdateDungeonStage;
    public event Action<ServerUserGameInfoData> OnUpdateParty;
    public event Action<ServerUserFatigueData> OnUseFatigue;

    public void Init()
    {
        API = new ApiService();
    }

    public async Task InitData()
    {
        if (!_isAlreadyInit)
        {
            _isAlreadyInit = true;
            await API.PostData<object, object>($"/user/sign-in", new object());
            User = await API.GetData<ServerUserGameInfoData>($"/user-game-info/{"UID0000001"}");
            UserMonsterDataList = await API.GetData<List<ServerUserMonsterData>>($"/user-monster/user/{User.UserId}");
            UserInventoryList = await API.GetData<List<ServerUserInventoryData>>($"/user-inventory/{User.UserId}");
            UserEggDataList = await API.GetData<List<ServerUserEggData>>($"/user-egg/{User.UserId}");
            UserIncubatorList = await API.GetData<List<ServerIncubatorData>>($"/user-incubator/{User.UserId}");
            UserDungeonProgressList = await API.GetData<List<ServerUserDungeonProgressData>>($"/user-dungeon-progress/{User.UserId}");
            UserFatigueData = await API.GetData<ServerUserFatigueData>($"/user-game-info/current-fatigue/{User.UserId}");
            CompletedQuestDataList = await API.GetData<List<ServerCompletedQuestData>>($"/quest/completed/{User.UserId}");
            SubtaskList = await API.GetData<List<ServerSubtaskData>>($"/quest/task/{User.UserId}");
        }
        else
        {
            await Task.CompletedTask;
        }
    }

    public async Task UpdatePartnerMonster(UpdatePartnerMonsterBody body)
    {
        await API.PostData<object, UpdatePartnerMonsterBody>("/user-game-info/update-partner-monster", body, onSuccess: (obj) =>
        {
            User.PartnerMonsterId = body.partnerMonsterId;
            OnUpdatePartnerMonster?.Invoke(body);
        }, onFailure: (message) =>
        {
            Logging.LogError(message);
        });
    }

    public async Task UserMonsterLevelUp(UserMonsterLevelUpBody body)
    {
        await API.PostData<UserMonsterLevelUpBody, ServerUserMonsterData>("/user-monster/level-up", body,
        onSuccess: (obj) =>
        {
            GameManager.Instance.UserMonsterManager.LevelUpMonster(obj.Id);
            GameManager.Instance.UserMonsterManager.UpdateLevelUpMonsterList(obj, body);
            OnUserMonsterLevelUp?.Invoke(body);
        },
        onFailure: (message) =>
        {
            Logging.LogError(message);
        });
    }

    public async Task StartHatchEgg(StartHatchingBody body)
    {
        await API.PostData<StartHatchingBody, ServerIncubatorData>("/user-incubator/start-hatching", body,
        onSuccess: (obj) =>
        {
            GameManager.Instance.HatchingManager.SetIncubator(obj);

            OnStartHatching?.Invoke(obj);
        },
        onFailure: (message) =>
        {
            Logging.LogError(message);
        });
    }

    public async Task EndHatchEgg(HatchEggBody body)
    {
        await API.PostData<HatchEggBody, ServerUserMonsterData>("/user-incubator/hatch-egg", body,
        onSuccess: (obj) =>
        {
            OnHatchEgg?.Invoke(obj);
        },
        onFailure: (message) =>
        {
            Logging.LogError(message);
        });
    }

    public async Task UpdateDungeonStage(UpdateStageBody body)
    {
        await API.PostData<UpdateStageBody, ServerUserDungeonProgressData>("/user-dungeon-progress/update-stage", body,
        onSuccess: (obj) =>
        {
            OnUpdateDungeonStage?.Invoke(obj);
        },
        onFailure: (message) =>
        {
            Logging.LogError(message);
        });
    }

    public async Task UpdateParty(UpdatePartyBody body)
    {
        await API.PostData<UpdatePartyBody, ServerUserGameInfoData>("/user-game-info/update-party", body,
        onSuccess: (obj) =>
        {
            User.PartnerMonsterIds = obj.PartnerMonsterIds;
            OnUpdateParty?.Invoke(obj);
        },
        onFailure: (message) =>
        {
            Logging.LogError(message);
        });
    }

    public async Task UseFatigueAfterDungeon(UpdateFatigueBody body)
    {
        await API.PostData<UpdateFatigueBody, ServerUserFatigueData>("/user-game-info/use-fatigue", body,
        onSuccess: (obj) =>
        {
            UserFatigueData = obj;

            OnUseFatigue?.Invoke(obj);
        },
        onFailure: (message) =>
        {
            Logging.LogError(message);
        });
    }

    public void UpdateSubtaskCurrenSuccessValue(UpdateValueBody body)
    {
        _queueManager.EnqueueRequest(async () =>
        {
            await API.PostData<UpdateValueBody, ServerSubtaskData>("/quest/task/update-value", body,
            onSuccess: (obj) =>
            {
                ServerSubtaskData data = SubtaskList.Find(x => x.Id == obj.Id);
                if (data == null)
                {
                    SubtaskList.Add(obj);
                }

                data = obj;
            },
            onFailure: (message) =>
            {
                Logging.LogError(message);
            });
        });
    }

    /*/// <summary>
    /// 버튼 눌리면 퀘스트 완료 서버에 보내기
    /// 사용 X, 퀘스트 + 리워드 + 경험치로 통합
    /// </summary>
    /// <returns></returns>
    public async Task CompleteQuest(AddCompletedBody body)
    {
        await API.PostData<AddCompletedBody, ServerCompletedQuestData>("/quest/add-completed", body,
        onSuccess: (obj) =>
        {
            CompletedQuestDataList.Add(obj);
        },
        onFailure: (message) =>
        {
            Debug.LogError(message);
        });
    }*/

    /// <summary>
    /// 아이템, 알 리워드 서버에 보내기
    /// 던전 리워드에서 사용가능
    /// </summary>
    /// <returns></returns>
    public async Task PostReward(RewardBody body)
    {
        await API.PostData<RewardBody, ServerUserInventoryData>("/user/reward", body,
        onSuccess: (obj) =>
        {

        },
        onFailure: (message) =>
        {
            Logging.LogError(message);
        });
    }


    /// <summary>
    /// 레벨업 경험치 서버에 보내기
    /// 던전 리워드에서 사용가능
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    public async Task PostLevelUp(LevelUpBody body)
    {
        await API.PostData<LevelUpBody, ServerUserData>("/user-game-info/level-up", body,
        onSuccess: (obj) =>
        {

        },
        onFailure: (message) =>
        {
            Logging.LogError(message);
        });
    }

    /// <summary>
    /// 퀘스트 완료 + 리워드 + 경험치 서버에 보내기
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    public async Task PostCompletedAndReward(CompletedAndRewardBody body)
    {
        await API.PostData<CompletedAndRewardBody, ServerAddedReward>("/quest/completed-and-reward", body,
        onSuccess: (obj) =>
        {
            if (obj.AddedEgg != null)
            {
                ServerUserEggData egg = new ServerUserEggData
                {
                    Id = obj.AddedEgg.Id,
                    UserId = obj.AddedEgg.UserId,
                    EggId = obj.AddedEgg.EggId,
                    AcquiredAt = obj.AddedEgg.AcquiredAt
                };

                UserEggDataList.Add(egg);
            }
        },
        onFailure: (message) =>
        {
            Logging.LogError(message);
        });
    }
}

