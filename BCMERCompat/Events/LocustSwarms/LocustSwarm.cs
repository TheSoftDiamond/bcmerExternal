#nullable enable

using BrutalCompanyMinus;
using BrutalCompanyMinus.Minus;

namespace TemporalStormWeather.Compat.BCMERCompat.Events.LocustSwarm;

internal class LocustSwarm : MEvent
{
    private static readonly string[] LocustEnemyNames =
    [
        "LocustEnemy",
        "SawbladeLocustEnemy",
        "CorruptLocustEnemy"
    ];

    private static readonly Scale NoSpawn = new(0f, 0f, 0f, 0f);
    private static readonly Scale InsideRarity = new(50f, 0.5f, 50f, 100f);
    private static readonly Scale LocustMin = new(5f, 0.06f, 5f, 11f);
    private static readonly Scale LocustMax = new(8f, 0.13f, 8f, 21f);
    private static readonly Scale SawbladeLocustMin = new(2f, 0.025f, 2f, 4f);
    private static readonly Scale SawbladeLocustMax = new(4f, 0.05f, 4f, 9f);
    private static readonly Scale CorruptLocustMin = new(2f, 0.025f, 2f, 4f);
    private static readonly Scale CorruptLocustMax = new(4f, 0.055f, 4f, 9f);

    public override string Name()
    {
        return nameof(LocustSwarm);
    }

    public override void Initalize()
    {
        Weight = 2;
        Descriptions =
        [
            "Locusts are swarming the facility"
        ];
        ColorHex = "#B87333";
        Type = EventType.Bad;
        Aliases = ["LocustSwarm"];
        monstersToSpawn =
        [
            new MonsterEvent(LocustEnemyNames[0], InsideRarity, NoSpawn, LocustMin, LocustMax, NoSpawn, NoSpawn),
            new MonsterEvent(LocustEnemyNames[1], InsideRarity, NoSpawn, SawbladeLocustMin, SawbladeLocustMax, NoSpawn, NoSpawn),
            new MonsterEvent(LocustEnemyNames[2], InsideRarity, NoSpawn, CorruptLocustMin, CorruptLocustMax, NoSpawn, NoSpawn)
        ];
    }

    public override bool AddEventIfOnly()
    {
        for (int i = 0; i < LocustEnemyNames.Length; i++)
        {
            if (!Assets.EnemyList.ContainsKey(LocustEnemyNames[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override void Execute()
    {
        ExecuteAllMonsterEvents();
        Utils.Log.Debug("LocustSwarm event started");
    }
}
