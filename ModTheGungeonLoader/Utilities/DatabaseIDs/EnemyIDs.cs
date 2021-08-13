using Dungeonator;
using Gungeon.Debug;
using System.Reflection;

namespace Gungeon.Utilities
{
    /// <summary>
    /// Every single Enemy GUID to spawn.
    /// </summary>
    public static class EnemyIDs
    {
        /// <summary>
        /// Spawn an enemy by an existing prefab.
        /// </summary>
        /// <param name="actorPrefab"></param>
        /// <param name="room"></param>
        /// <param name="spawnPoint"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static AIActor Spawn(AIActor actorPrefab, RoomHandler room, IntVector2? spawnPoint = null, SpawnSettings? settings = null)
        {
            if (actorPrefab == null)
            {
                $"Cannot spawn null actor prefab".LogInternal(Assembly.GetCallingAssembly(), Debug.Logger.LogTypes.error);
                return null;
            }
            else if (room == null)
            {
                $"Room must be valid to spawn enemy.".LogInternal(Assembly.GetCallingAssembly(), Debug.Logger.LogTypes.error);
                return null;
            }

            if (spawnPoint == null)
                spawnPoint = room.GetCenterCell();

            if (settings == null)
                settings = SpawnSettings.DefaultSettings;

            return AIActor.Spawn(actorPrefab, spawnPoint.Value, room, settings.Value.correctForWalls, settings.Value.spawnAnimation, settings.Value.autoEngage);
        }

        /// <summary>
        /// Spawn an enemy by guid
        /// </summary>
        /// <param name="guid">Enemy GUID</param>
        /// <param name="room">Source Room</param>
        /// <param name="spawnPoint">Point to spawn enemy.</param>
        /// <param name="settings">Spawn Settings.</param>
        /// <returns></returns>
        public static AIActor Spawn(string guid, RoomHandler room, IntVector2? spawnPoint = null, SpawnSettings? settings = null)
        {
            AIActor actor = EnemyDatabase.GetOrLoadByGuid(guid);

            if (actor == null)
            {
                $"Invalid ID while spawning enemy : '{guid}'".LogInternal(Assembly.GetCallingAssembly(), Debug.Logger.LogTypes.error);
                return null;
            }

            return Spawn(actor, room, spawnPoint, settings);
        }

        /// <summary>
        /// Get an enemy by their guid or name
        /// </summary>
        /// <param name="guidOrName">GUID</param>
        /// <param name="name">Use <paramref name="guidOrName"/> as a name parameter</param>
        /// <returns></returns>
        public static AIActor GetEnemy(string guidOrName, bool name = false)
        {
            AIActor actor = name ? EnemyDatabase.GetOrLoadByName(guidOrName) : EnemyDatabase.GetOrLoadByGuid(guidOrName);

            if (actor == null)
            {
                $"Could not get enemy : '{guidOrName}'".LogInternal(Assembly.GetCallingAssembly(), Logger.LogTypes.error);
                return null;
            }

            return actor;
        }



        /// <summary>
        /// Random enemy.
        /// </summary>
        /// <returns></returns>
        public static AIActor GetEnemy()
        {
            return EnemyDatabase.Instance.Entries.Random(x => x.GetPrefab<AIActor>() != null).GetPrefab<AIActor>();
        }

        /// <summary>
        /// Spawn settings for spawning an enemy.
        /// </summary>
        public struct SpawnSettings
        {
            /// <summary>
            /// Correct the spawn point for walls?
            /// </summary>
            public bool correctForWalls;
            /// <summary>
            /// How should the enemy spawn in?
            /// </summary>
            public AIActor.AwakenAnimationType spawnAnimation;

            /// <summary>
            /// Should the enemy automatically engage the player?
            /// </summary>
            public bool autoEngage;

            /// <summary>
            /// Default spawn settings.
            /// </summary>
            public static SpawnSettings DefaultSettings => new SpawnSettings()
            {
                autoEngage = true,
                correctForWalls = false,
                spawnAnimation = AIActor.AwakenAnimationType.Default
            };
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string ak47_bullet_kin = "db35531e66ce41cbb81d507a34366dfe";
        public const string rubber_kin = "6b7ef9e5d05b4f96b04f05ef4a0d1b18";
        public const string red_shotgun_kin = "128db2f0781141bcb505d8f00f9e4d47";
        public const string gatling_gull = "ec6b674e0acd4553b47ee94493d66422";
        public const string veteran_bullet_kin = "70216cae6c1346309d86d4a0b4603045";
        public const string grenade_kin = "4d37ce3d666b4ddda8039929225b7ede";
        public const string blobulin = "42be66373a3d4d89b91a35c9ff8adfec";
        public const string bullet_kin = "01972dee89fc4404a5c408d50007dad5";
        public const string bandana_bullet_kin = "88b6b6a93d4b4234a67844ef4728382c";
        public const string arrow_head = "05891b158cd542b1a5f3df30fb67a7ff";
        public const string blobulon = "0239c0680f9f467dbe5c4aab7dd1eca6";
        public const string hollowpoint = "4db03291a12144d69fe940d5a01de376";
        public const string skusket = "af84951206324e349e1f13f9b7b60c1a";
        public const string blobuloid = "042edb1dfb614dc385d5ad1b010f2ee3";
        public const string beholster = "4b992de5b4274168a8878ef9bf7ea36b";
        public const string bullet_kin_fakery = "b5503988e3684e8fa78274dd0dda0bf5";
        public const string veteran_bullet_kin_fakery = "06a82532447247f9ada1940d079a31a7";
        public const string unused_muzzle_flare = "98e52539e1964749a8bbce0fe6a85d6b";
        public const string treadnaught = "fa76c8cfdf1c4a88b55173666b4bc7fb";
        public const string treadnaughts_bullet_kin = "df7fb62405dc4697b7721862c7b6b3cd";
        public const string beadie = "7b0b1b6d9ce7405b86b75ce648025dd6";
        public const string muzzle_wisp = "ffdc8680bdaa487f8f31995539f74265";
        public const string blue_shotgun_kin = "b54d89f9e802455cbb2b8a96a31e8259";
        public const string cubulon = "864ea5a6a9324efc95a0dd2407f42810";
        public const string friendly_bullet_kin = "c1757107b9a44f0c823a707adeb4ae7e";
        public const string gunjurer = "c4fba8def15e47b297865b18e36cbef8";
        public const string portable_turret = "998807b57e454f00a63d67883fcf90d6";
        public const string tutorial_turret = "e667fdd01f1e43349c03a18e5b79e579";
        public const string ser_manuel = "fc809bd43a4d41738a62d7565456622c";
        public const string tutorial_bullet_kin = "b08ec82bef6940328c7ecd9ffc6bd16c";
        public const string bunker = "8817ab9de885424d9ba83455ead5ffef";
        public const string bullet_king = "ffca09398635467da3b1f4a54bcfda80";
        public const string bullet_kings_toadie = "b5e699a0abb94666bda567ab23bd91c4";
        public const string veteran_shotgun_kin = "2752019b770f473193b08b4005dc781f";
        public const string high_gunjurer = "9b2cf2949a894599917d4d391a0b7394";
        public const string spent = "249db525a9464e5282d02162c88e0357";
        public const string blizzbulon = "022d7c822bc146b58fe3b0287568aaa2";
        public const string mountain_cube = "f155fd2759764f4a9217db29dd21b7eb";
        public const string skullet = "336190e29e8a4f75ab7486595b700d4a";
        public const string gummy = "5288e86d20184fa69c91ceb642d31474";
        public const string skullmet = "95ec774b5a75467a9ab05fa230c0c143";
        public const string smiley = "ea40fcc863d34b0088f490f4e57f8913";
        public const string shades = "c00390483f394a849c36143eb878998f";
        public const string old_king = "5729c8b5ffa7415bb3d01205663a33ef";
        public const string super_space_turtle = "3a077fa5872d462196bb9a3cb1af02a3";
        public const string wallmonger = "f3b04a067a65492f8b279130323b41f0";
        public const string dragun = "465da2bb086a4a88a803f79fe3a27677";
        public const string brown_chest_mimic = "2ebf8ef6728648089babb507dec4edb7";
        public const string bullat = "2feb50a6a40f4f50982e89fd276f6f15";
        public const string shotgat = "2d4f8b5404614e7d8b235006acde427a";
        public const string shelleton = "21dd14e5ca2a4a388adab5b11b69a1e1";
        public const string bookllet = "c0ff3744760c4a2eb0bb52ac162056e6";
        public const string grenat = "b4666cb6ef4f4b038ba8924fd8adf38f";
        public const string high_priest = "6c43fddfd401456c916089fdd1c99b1c";
        public const string blue_chest_mimic = "d8d651e3484f471ba8a2daa4bf535ce6";
        public const string green_chest_mimic = "abfb454340294a0992f4173d6e5898a8";
        public const string door_lord = "9189f46c47564ed588b9108965f975c9";
        public const string cannonbalrog = "5e0af7f7d9de4755a68d2fd3bbc15df4";
        public const string blue_bookllet = "6f22935656c54ccfb89fca30ad663a64";
        public const string green_bookllet = "a400523e535f41ac80a43ff6b06dc0bf";
        public const string spirat = "7ec3e8146f634c559a7d58b19191cd43";
        public const string dog = "c07ef60ae32b404f99e294a6f9acba75";
        public const string skusket_head = "c2f902b7cbe745efb3db4399927eab34";
        public const string gatling_gull_ally = "538669d3b2cd4edca2e3699812bcf2b6";
        public const string tazie = "98fdf153a4dd4d51bf0bafe43f3c77ff";
        public const string black_skusket = "1cec0cdf383e42b19920787798353e46";
        public const string det = "ac986dabc5a24adab11d48a4bccf4cb1";
        public const string x_det = "48d74b9c65f44b888a94f9e093554977";
        public const string minelet = "3cadf10c489b461f9fb8814abc1a09c1";
        public const string boss_template = "7ee0a3fbb3dc417db4c3073ba23e1be8";
        public const string gorgun = "c367f00240a64d5d9f3c26484dc35833";
        public const string poisbulon = "e61cab252cfb435db9172adc96ded75f";
        public const string poisbuloid = "fe3fe59d867347839824d5d9ae87f244";
        public const string poisbulin = "b8103805af174924b578c98e95313074";
        public const string ammoconda = "da797878d215453abba824ff902e21b4";
        public const string gun_nut = "ec8ea75b557d4e7b8ceeaacdf6f8238c";
        public const string gigi = "ed37fa13e0fa4fcf8239643957c51293";
        public const string gunzookie = "6e972cd3b11e4b429b888b488e308551";
        public const string LeadWizard = "cf2b7021eac44e3f95af07db9a7c442c";
        public const string gunsinger = "8a9e9bedac014a829a48735da6daf3da";
        public const string t_bulon = "ccf6d241dad64d989cbcaca2a8477f01";
        public const string aged_gunsinger = "c50a862d19fc4d30baeba54795e8cb93";
        public const string cubulead = "0b547ac6b6fc4d68876a241a88f5ca6a";
        public const string creech = "37340393f97f41b2822bc02d14654172";
        public const string sniper_shell = "31a3ea0c54a745e182e22ea54844a82d";
        public const string professional = "c5b11bfc065d417b9c4d03a5e385fe2c";
        public const string wizbang = "43426a2e39584871b287ac31df04b544";
        public const string coaler = "9d50684ce2c044e880878e86dbada919";
        public const string gat = "9b4fb8a2a60a457f90dcf285d34143ac";
        public const string fungun = "f905765488874846b7ff257ff81d6d0c";
        public const string spogre = "eed5addcc15148179f300cc0d9ee7f94";
        public const string fallen_bullet_kin = "5f3abc2d561b4b9c9e72b879c6f10c7e";
        public const string shotgrub = "044a9f39712f456597b9762893fbc19c";
        public const string lead_cube = "33b212b856b74ff09252bf4f2e8b8c57";
        public const string flesh_cube = "3f2026dc3712490289c4658a2ba4a24b";
        public const string shroomer = "e5cffcfabfae489da61062ea20539887";
        public const string ammomancer = "b1540990a4f1480bbcb3bea70d67f60d";
        public const string spectre = "56f5a0f2c1fc4bc78875aea617ee31ac";
        public const string lore_gunjurer = "56fb939a434140308b8f257f0f447829";
        public const string muzzle_flare = "d8a445ea4d944cc1b55a40f22821ae69";
        public const string dr_wolfs_monster = "8d441ad4e9924d91b6070d5b3438d066";
        public const string dr_wolf = "ce2d2a0dced0444fb751b262ec6af08a";
        public const string lich = "cd88c3ce60c442e9aa5b3904d31652bc";
        public const string psychoman = "575a37abca8d414d89c4e251dd606050";
        public const string bishop = "5d045744405d4438b371eb5ed3e2cdb2";
        public const string blobulord = "1b5810fafbec445d89921a4efb4e42b7";
        public const string shopkeeper_boss = "70058857b0a641a888ac4389bd10f455";
        public const string blockner = "bb73eeeb9e584fbeaf35877ec176de28";
        public const string tsar_bomba = "39de9bd6a863451a97906d949c103538";
        public const string interdimensional_horror = "dc3cd41623d447aeba77c77c99598426";
        public const string marines_past_imp = "a9cc6a4e9b3d46ea871e70a03c9f77d4";
        public const string convicts_past_soldier = "556e9f2a10f9411cb9dbfd61e0e0f1e1";
        public const string black_stache = "8b913eea3d174184be1af362d441910d";
        public const string diagonal_x_det = "c5a0fd2774b64287bf11127ca59dd8b4";
        public const string vertical_det = "b67ffe82c66742d1985e5888fd8e6a03";
        public const string diagonal_det = "d9632631a18849539333a92332895ebd";
        public const string horizontal_det = "1898f6fe1ee0408e886aaf05c23cc216";
        public const string vertical_x_det = "abd816b0bcbf4035b95837ca931169df";
        public const string horizontal_x_det = "07d06d2b23cc48fe9f95454c839cb361";
        public const string r2g2 = "1ccdace06ebd42dc984d46cb1f0db6cf";
        public const string caterpillar = "d375913a61d1465f8e4ffcf4894e4427";
        public const string hm_absolution = "b98b10fca77d469e80fb45f3c5badec5";
        public const string draguns_knife = "78eca975263d4482a4bfa4c07b32e252";
        public const string lord_of_the_jammed = "0d3f7c641557426fbac8596b61c9fb45";
        public const string tombstoner = "cf27dd464a504a428d87a8b2560ad40a";
        public const string megalich = "68a238ed6a82467ea85474c595c49c6e";
        public const string red_caped_bullet_kin = "fa6a7ac20a0e4083a4c2ee0d86f8bbf7";
        public const string infinilich = "7c5d5f09911e49b78ae644d2b50ff3bf";
        public const string tiny_blobulord = "d1c9781fdac54d9e8498ed89210a0238";
        public const string faster_tutorial_turret = "41ba74c517534f02a62f2e2028395c58";
        public const string cop = "705e9081261446039e1ed9ff16905d04";
        public const string chicken = "76bc43539fc24648bff4568c75c686d1";
        public const string ashen_bullet_kin = "1a78cfb776f54641b832e92c44021cf2";
        public const string ashen_shotgun_kin = "1bd8e49f93614e76b140077ff2e33f2b";
        public const string mutant_bullet_kin = "d4a9836f8ab14f3fadd0f597438b1f1f";
        public const string pig = "fe51c83b41ce4a46b42f54ab5f31e6d0";
        public const string poopulon = "116d09c26e624bca8cca09fc69c714b3";
        public const string poopulons_corn = "0ff278534abb4fbaaa65d3f638003648";
        public const string king_bullat = "1a4872dafdb34fd29fe8ac90bd2cea67";
        public const string spectre_gun_nut = "383175a55879441d90933b5c4e60cf6f";
        public const string bullet_shark = "72d2f44431da43b8a3bae7d8a114a46d";
        public const string agonizer = "3f6d6b0c4a7c4690807435c7b37c35a5";
        public const string lead_maiden = "cd4a4b7f612a4ba9a720b9f97c52f38c";
        public const string cardinal = "8bb5578fba374e8aae8e10b754e61d62";
        public const string shambling_round = "98ea2fe181ab4323ab6e9981955a9bca";
        public const string hunters_past_dog = "ededff1deaf3430eaf8321d0c6b2bd80";
        public const string bloodbulon = "062b9b64371e46e195de17b6f10e47c8";
        public const string black_chest_mimic = "6450d20137994881aff0ddd13e3d40c8";
        public const string red_chest_mimic = "d8fd592b184b4ac9a3be217bc70912a2";
        public const string gun_cultist = "57255ed50ee24794b7aac1ac3cfb8a95";
        public const string revolvenant = "d5a7b95774cd41f080e517bea07bf495";
        public const string gunreaper = "88f037c3f93b4362a040a87b30770407";
        public const string pot_fairy = "c182a5cb704d460d9d099a47af49c913";
        public const string blank_companion = "5695e8ffa77c4d099b4d9bd9536ff35e";
        public const string great_bullet_shark = "b70cbd875fea498aa7fd14b970248920";
        public const string apprentice_gunjurer = "206405acad4d4c33aac6717d184dc8d4";
        public const string mutant_shotgun_kin = "7f665bd7151347e298e4d366f8818284";
        public const string kill_pillars = "3f11bbbc439c4086a180eb0fb9990cb4";
        public const string snake = "1386da0f42fb4bcabc5be8feb16a7c38";
        public const string gummy_spent = "e21ac9492110493baef6df02a2682a0d";
        public const string old_kings_toadie = "02a14dec58ab45fb8aacde7aacd25b01";
        public const string summoned_treadnaughts_bullet_kin = "47bdfec22e8e4568a619130a267eab5b";
        public const string ser_junkan = "c6c8e59d0f5d41969c74e802c9d67d07";
        public const string test_dummy = "5fa8c86a65234b538cd022f726af2aea";
        public const string mine_flayer = "8b0dd96e2fe74ec7bebc1bc689c0008a";
        public const string mine_flayers_bell = "78a8ee40dff2477e9c2134f6990ef297";
        public const string key_bullet_kin = "699cd24270af4cd183d671090d8323a1";
        public const string mine_flayers_claymore = "566ecca5f3b04945ac6ce1f26dedbf4f";
        public const string robots_past_human_1 = "1398aaccb26d42f3b998c367b7800b85";
        public const string robots_past_human_2 = "9044d8e4431f490196ba697927a4e3d4";
        public const string robots_past_human_3 = "40bf8b0d97794a26b8c440425ec8cac1";
        public const string robots_past_human_4 = "3590db6c4eac474a93299a908cb77ab2";
        public const string jammomancer = "8b4a938cdbc64e64822e841e482ba3d2";
        public const string executioner = "b1770e0f1c744d9d887cc16122882b4f";
        public const string dynamite_kin = "c0260c286c8d4538a697c5bf24976ccf";
        public const string killithid = "3e98ccecf7334ff2800188c417e67c15";
        public const string jamerlengo = "ba657723b2904aa79f9e51bce7d23872";
        public const string bombshee = "19b420dec96d4e9ea4aebc3398c0ba7a";
        public const string last_human = "880bbe4ce1014740ba6b4e2ea521e49d";
        public const string robots_past_terminator = "12a054b8a6e549dcac58a82b89e319e5";
        public const string robots_past_critter_1 = "95ea1a31fc9e4415a5f271b9aedf9b15";
        public const string robots_past_critter_2 = "42432592685e47c9941e339879379d3a";
        public const string robots_past_critter_3 = "4254a93fc3c84c0dbe0a8f0dddf48a5a";
        public const string chain_gunner = "463d16121f884984abe759de38418e48";
        public const string cucco = "7bd9c670f35b4b8d84280f52a5cc47f6";
        public const string hooded_bullet = "844657ad68894a4facb1b8e1aef1abf9";
        public const string agunim = "2ccaa1b7ae10457396a1796decda9cf6";
        public const string cannon = "39dca963ae2b4688b016089d926308ab";
        public const string shadow_agunim = "db97e486ef02425280129e1e27c33118";
        public const string ammoconda_ball = "f38686671d524feda75261e469f30e0b";
        public const string chance_bullet_kin = "a446c626b56d4166915a4e29869737fd";
        public const string grip_master = "22fc2c2c45fb47cf9fb5f7b043a70122";
        public const string phaser_spider = "98ca70157c364750a60f5e0084f9d3e2";
        public const string wall_mimic = "479556d05c7c44f3b6abb3b2067fc778";
        public const string chancebulon = "1bc2a07ef87741be90c37096910843ab";
        public const string tarnisher = "475c20c1fd474dfbad54954e7cba29c1";
        public const string misfire_beast = "45192ff6d6cb43ed8f1a874ab6bef316";
        public const string dragun_egg_slimeguy = "8b43a5c59b854eb780f9ab669ec26b7a";
        public const string blockner_rematch = "edc61b105ddd4ce18302b82efdc47178";
        public const string rat_candle = "14ea47ff46b54bb4a98f91ffcffb656d";
        public const string rat = "6ad1cafc268f4214a101dca7af61bc91";
        public const string rat_chest_mimic = "ac9d345575444c9a8d11b799e8719be0";
        public const string pedestal_mimic = "796a7ed4ad804984859088fc91672c7f";
        public const string mouser = "be0683affb0e41bbb699cb7125fdded6";
        public const string candle_guy = "eeb33c3a5a8e4eaaaaf39a743e8767bc";
        public const string metal_cube_guy = "ba928393c8ed47819c2c5f593100a5bc";
        public const string fusebot = "4538456236f64ea79f483784370bc62f";
        public const string bullet_kings_toadie_revenge = "d4dd2b2bbda64cc9bcec534b4e920518";
        public const string phoenix = "11a14dbd807e432985a89f69b5f9b31e";
        public const string pig_synergy = "86237c6482754cd29819c239403a2de7";
        public const string blank_companion_synergy = "ad35abc5a3bf451581c3530417d89f2c";
        public const string cop_android = "640238ba85dd4e94b3d6f68888e6ecb8";
        public const string raccoon = "e9fa6544000942a79ad05b6e4afb62db";
        public const string dog_synergy_1 = "ebf2314289ff4a4ead7ea7ef363a0a2e";
        public const string dog_synergy_2 = "ab4a779d6e8f429baafa4bf9e5dca3a9";
        public const string super_space_turtle_synergy = "9216803e9c894002a4b931d7ea9c6bdf";
        public const string turtle = "cc9c41aa8c194e17b44ac45f993dd212";
        public const string payday_shooter_01 = "45f5291a60724067bd3ccde50f65ac22";
        public const string payday_shooter_02 = "41ab10778daf4d3692e2bc4b370ab037";
        public const string payday_shooter_03 = "2976522ec560460c889d11bb54fbe758";
        public const string turkey = "6f9c28403d3248c188c391f5e40774c5";
        public const string baby_mimic = "e456b66ed3664a4cb590eab3a8ff3814";
        public const string resourceful_rat = "6868795625bd46f3ae3e4377adce288b";
        public const string resourceful_rat_mech = "4d164ba3f62648809a4a82c90fc22cae";
        public const string dragun_advanced = "05b8afe0b6cc4fffa9dc6036fa24c8ec";
        public const string dragun_knife_advanced = "2e6223e42e574775b56c6349921f42cb";
        public const string musket_kin = "226fd90be3a64958a5b13cb0a4f43e97";
        public const string bullet_kin_gal_titan_boss = "df4e9fedb8764b5a876517431ca67b86";
        public const string bullet_kin_titan_boss = "1f290ea06a4c416cabc52d6b3cf47266";
        public const string bullet_kin_titan = "c4cf0620f71c4678bb8d77929fd4feff";
        public const string bullet_kin_pirate = "6f818f482a5c47fd8f38cce101f6566c";
        public const string bullet_kin_fish = "143be8c9bbb84e3fb3ab98bcd4cf5e5b";
        public const string bullet_kin_fish_blue = "06f5623a351c4f28bc8c6cda56004b80";
        public const string bullet_kin_broccoli = "ff4f54ce606e4604bf8d467c1279be3e";
        public const string bullet_kin_knight = "39e6f47a16ab4c86bec4b12984aece4c";
        public const string bullet_kin_kaliber = "f020570a42164e2699dcf57cac8a495c";
        public const string bullet_kin_candle = "37de0df92697431baa47894a075ba7e9";
        public const string bullet_kin_cowboy = "5861e5a077244905a8c25c2b7b4d6ebb";
        public const string bullet_kin_officetie = "906d71ccc1934c02a6f4ff2e9c07c9ec";
        public const string bullet_kin_officesuit = "9eba44a0ea6c4ea386ff02286dd0e6bd";
        public const string bullet_kin_mech = "2b6854c0849b4b8fb98eb15519d7db1c";
        public const string bullet_kin_vest = "05cb719e0178478685dc610f8b3e8bfc";
        public const string dynamite_kin_office = "5f15093e6f684f4fb09d3e7e697216b4";
        public const string cylinder = "d4f4405e0ff34ab483966fd177f2ece3";
        public const string cylinder_red = "534f1159e7cf4f6aa00aeea92459065e";
        public const string gunzookie_office = "80ab6cd15bfc46668a8844b2975c6c26";
        public const string bullat_gargoyle = "981d358ffc69419bac918ca1bdf0c7f7";
        public const string snake_office = "e861e59012954ab2b9b6977da85cb83c";
        public const string agunim_helicopter = "41ee1c8538e8474a82a74c4aff99c712";
        public const string cactus_kin = "3b0bd258b4c9432db3339665cc61c356";
        public const string gigi_parrot = "4b21a913e8c54056bc05cafecf9da880";
        public const string tablet_bookllet = "78e0951b097b46d89356f004dda27c42";
        public const string necronomicon_bookllet = "216fd3dfb9da439d9bd7ba53e1c76462";
        public const string cowboy_shotgun_kin = "ddf12a4881eb43cfba04f36dd6377abb";
        public const string pirate_shotgun_kin = "86dfc13486ee4f559189de53cfb84107";
        public const string lead_maiden_fridge = "9215d1a221904c7386b481a171e52859";
        public const string baby_shelleton = "3f40178e10dc4094a1565cd4fdc4af56";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}
