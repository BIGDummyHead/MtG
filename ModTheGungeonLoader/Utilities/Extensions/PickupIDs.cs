using Gungeon.Debug;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gungeon.Utilities
{
    /// <summary>
    /// All object ids 
    /// </summary>
    public static class PickupIDs
    {
        /// <summary>
        /// Every object.
        /// </summary>
        public static List<PickupObject> AllObjects => PickupObjectDatabase.Instance.Objects;

        /// <summary>
        /// A completely random gun, every single time
        /// </summary>
        public static Gun RandomGun => PickupObjectDatabase.GetRandomGun();

        /// <summary>
        /// Get a random passive.
        /// </summary>
        public static PassiveItem RandomPassive
        {
            get
            {
                List<PassiveItem> passives = new List<PassiveItem>((IEnumerable<PassiveItem>)AllObjects.Where(x => x is PassiveItem));

                int pick = new System.Random().Next(0, passives.Count);

                return passives[pick];
            }
        }

        /// <summary>
        /// Get a random gun of quality.
        /// </summary>
        /// <param name="excludeIDs">Ids to exclude</param>
        /// <param name="qualities">Gun qualities</param>
        /// <returns></returns>
        public static Gun GetRandomGunOfQuality(List<int> excludeIDs, params PickupObject.ItemQuality[] qualities)
        {
            System.Random ran = new System.Random();
            return PickupObjectDatabase.GetRandomGunOfQualities(ran, excludeIDs, qualities);
        }

        /// <summary>
        /// Get an item by name | <see cref="StringComparison.OrdinalIgnoreCase"/> is used.
        /// </summary>
        /// <param name="name">Item's name</param>
        /// <param name="byDisplayName">Use display name</param>
        /// <returns></returns>
        public static PickupObject GetItem(string name, bool byDisplayName = false)
        {
            return byDisplayName ? PickupObjectDatabase.GetByEncounterName(name) : PickupObjectDatabase.GetByName(name);
        }

        /// <summary>
        /// Get an item via ID.
        /// </summary>
        /// <param name="id">Object ID</param>
        /// <returns></returns>
        public static PickupObject GetItem(int id)
        {
            return PickupObjectDatabase.GetById(id);
        }

        /// <summary>
        /// Create a table from a singular <see cref="PickupObject"/> and add to the pool in one method.
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static LootTableBuilder ToTable(this PickupObject pool, params PickupObject[] add)
        {
            LootTableBuilder builder = new LootTableBuilder();
            builder.PoolItem(pool);

            foreach (PickupObject o in add)
            {
                builder.PoolItem(o);
            }

            return builder;
        }

        /// <summary>
        /// Give an item to a Player
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <param name="player">Player to pickup item</param>
        /// <returns></returns>
        /// <remarks>Player defaults to the current player in game.</remarks>
        public static PickupObject GiveItem(int id, PlayerController player = null)
        {
            player = player ?? ModUtilities.CurrentPlayer;

            PickupObject pick = PickupIDs.GetItem(id);

            if (pick is Gun)
            {
                player.inventory.AddGunToInventory(pick as Gun, true);

                return pick;
            }
            else if (pick is PassiveItem)
            {
                (pick as PassiveItem).Pickup(player);
                return pick;
            }
            else if (pick is PlayerItem)
            {
                PlayerItem item = pick as PlayerItem;
                item.Pickup(player);

                return pick;
            }

            "Item could not be found.".LogError();

            return null;
        }

        /// <summary>
        /// Give an item to a player
        /// </summary>
        /// <param name="itemName">Name of item</param>
        /// <param name="useDisplayName">Use display name of item to get.</param>
        /// <param name="player">Player to pickup item.</param>
        /// <returns></returns>
        /// <remarks>Player defaults to the current player in game. 
        /// Read comments if any for spawn names of items.</remarks>
        public static PickupObject GiveItem(string itemName, bool useDisplayName = false, PlayerController player = null)
        {
            player = player ?? ModUtilities.CurrentPlayer;

            int id = GetItem(itemName, useDisplayName).PickupObjectId;
            return GiveItem(id, player);
        }

        #region IDs
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const int magic_lamp = 0;
        public const int winchester = 1;
        public const int thompson = 2;
        public const int screecher = 3;
        public const int sticky_crossbow = 4;
        public const int awp = 5;
        public const int zorgun = 6;
        public const int barrel = 7;
        public const int bow = 8;
        public const int dueling_pistol = 9;
        public const int mega_douser = 10;
        public const int crossbow = 12;
        public const int thunderclap = 13;
        public const int bee_hive = 14;
        public const int ak47 = 15;
        public const int yari_launcher = 16;
        public const int heck_blaster = 17;
        public const int blooper = 18;
        public const int grenade_launcher = 19;
        public const int moonscraper = 20;
        public const int bsg = 21;
        public const int shades_revolver = 22;
        public const int dungeon_eagle = 23;
        public const int dart_gun = 24;
        public const int m1 = 25;
        public const int nail_gun = 26;
        public const int light_gun = 27;
        public const int mailbox = 28;
        public const int vertebraek47 = 29;
        public const int m1911 = 30;
        public const int klobbe = 31;
        public const int void_marshal = 32;
        public const int tear_jerker = 33;
        public const int smileys_revolver = 35;
        public const int megahand = 36;
        public const int serious_cannon = 37;
        public const int magnum = 38;
        public const int rpg = 39;
        public const int freeze_ray = 40;
        public const int heroine = 41;
        public const int trank_gun = 42;
        public const int machine_pistol = 43;
        public const int skull_spitter = 45;
        public const int jolter = 47;
        public const int sniper_rifle = 49;
        public const int saa = 50;
        public const int regular_shotgun = 51;
        public const int crescent_crossbow = 52;
        public const int au_gun = 53;
        public const int laser_rifle = 54;
        public const int void_shotgun = 55;
        /// <summary>
        /// Spawn name : 38_special
        /// </summary>
        public const int thirtyEight_special = 56;
        public const int alien_sidearm = 57;
        public const int void_core_assault_rifle = 58;
        public const int hegemony_rifle = 59;
        public const int demon_head = 60;
        public const int bundle_of_wands = 61;
        public const int colt_1851 = 62;
        public const int medkit = 63;
        public const int potion_of_lead_skin = 64;
        public const int knife_shield = 65;
        public const int proximity_mine = 66;
        public const int key = 67;
        public const int casing = 68;
        public const int bullet_time = 69;
        public const int unknown_2 = 70;
        public const int decoy = 71;
        public const int bubble_shield = 72;
        public const int half_heart = 73;
        /// <summary>
        /// Spawn name : 50_casing
        /// </summary>
        public const int fifty_casing = 74;
        public const int eye_jewel = 76;
        public const int supply_drop = 77;
        public const int ammo = 78;
        public const int makarov = 79;
        public const int budget_revolver = 80;
        public const int deck4rd = 81;
        public const int elephant_gun = 82;
        public const int unfinished_gun = 83;
        public const int vulcan_cannon = 84;
        public const int heart = 85;
        public const int marine_sidearm = 86;
        public const int gamma_ray = 87;
        public const int robots_right_hand = 88;
        public const int rogue_special = 89;
        public const int eye_of_the_beholster = 90;
        public const int h4mmer = 91;
        public const int stinger = 92;
        public const int old_goldie = 93;
        public const int mac10 = 94;
        public const int akey47 = 95;
        public const int m16 = 96;
        public const int polaris = 97;
        public const int patriot = 98;
        public const int rusty_sidearm = 99;
        public const int unicorn_horn = 100;
        public const int scope = 102;
        public const int ration = 104;
        public const int fortunes_favor = 105;
        public const int jetpack = 106;
        public const int raiden_coil = 107;
        public const int bomb = 108;
        public const int ice_bomb = 109;
        public const int magic_sweet = 110;
        public const int heavy_bullets = 111;
        public const int cartographers_ring = 112;
        public const int rocket_powered_bullets = 113;
        public const int bionic_leg = 114;
        public const int ballot = 115;
        public const int ammo_synthesizer = 116;
        public const int eyepatch = 118;
        public const int metronome = 119;
        public const int armor = 120;
        public const int disintegrator = 121;
        public const int blunderbuss = 122;
        public const int pulse_cannon = 123;
        public const int cactus = 124;
        public const int flame_hand = 125;
        public const int shotbow = 126;
        public const int junk = 127;
        public const int rube_adyne_mk2 = 128;
        public const int com4nd0 = 129;
        public const int glacier = 130;
        public const int utility_belt = 131;
        public const int ring_of_miserly_protection = 132;
        public const int backpack = 133;
        public const int ammo_belt = 134;
        public const int cog_of_battle = 135;
        public const int c4 = 136;
        public const int map = 137;
        public const int honeycomb = 138;
        public const int master_of_unlocking = 140;
        public const int rube_adyne_prototype = 142;
        public const int shotgun_full_of_hate = 143;
        public const int witch_pistol = 145;
        public const int dragunfire = 146;
        public const int placeable_key = 147;
        public const int lies = 148;
        public const int face_melter = 149;
        public const int t_shirt_cannon = 150;
        public const int the_membrane = 151;
        public const int the_kiln = 152;
        public const int shock_rifle = 153;
        public const int trashcannon = 154;
        public const int singularity = 155;
        public const int laser_lotus = 156;
        public const int big_iron = 157;
        public const int amulet_of_the_pit_lord = 158;
        public const int gundromeda_strain = 159;
        public const int gunknight_helmet = 160;
        public const int gunknight_greaves = 161;
        public const int gunknight_gauntlet = 162;
        public const int gunknight_armor = 163;
        public const int heart_synthesizer = 164;
        public const int oiled_cylinder = 165;
        public const int shelleton_key = 166;
        public const int bloody_eye = 167;
        public const int double_vision = 168;
        public const int black_hole_gun = 169;
        public const int ice_cube = 170;
        public const int ghost_bullets = 172;
        public const int pile_of_souls = 173;
        public const int potion_of_gun_friendship = 174;
        public const int tangler = 175;
        public const int gungeon_ant = 176;
        public const int alien_engine = 177;
        public const int crestfaller = 178;
        public const int proton_backpack = 179;
        public const int grasschopper = 180;
        public const int winchester_rifle = 181;
        public const int grey_mauser = 182;
        public const int ser_manuels_revolver = 183;
        public const int the_judge = 184;
        public const int machine_fist = 186;
        public const int disarming_personality = 187;
        public const int rolling_eye = 190;
        public const int ring_of_fire_resistance = 191;
        public const int bug_boots = 193;
        public const int fossilized_gun = 196;
        public const int pea_shooter = 197;
        public const int gunslingers_ashes = 198;
        public const int luxin_cannon = 199;
        public const int charmed_bow = 200;
        public const int portable_turret = 201;
        public const int sawed_off = 202;
        public const int cigarettes = 203;
        public const int irradiated_lead = 204;
        public const int poison_vial = 205;
        public const int charm_horn = 206;
        public const int plague_pistol = 207;
        public const int plunger = 208;
        public const int sense_of_direction = 209;
        public const int gunbow = 210;
        public const int ballistic_boots = 212;
        public const int lichy_trigger_finger = 213;
        public const int coin_crown = 214;
        public const int box = 216;
        public const int old_knights_shield = 219;
        public const int tutorial_ak47 = 221;
        public const int old_knights_helm = 222;
        public const int cold_45 = 223;
        public const int blank = 224;
        public const int ice_breaker = 225;
        public const int wristbow = 227;
        public const int particulator = 228;
        public const int hegemony_carbine = 229;
        public const int helix = 230;
        public const int gilded_hydra = 231;
        public const int space_friend = 232;
        public const int ibomb_companion_app = 234;
        public const int aged_bell = 237;
        public const int duct_tape = 239;
        public const int crutch = 240;
        public const int scattershot = 241;
        public const int napalm_strike = 242;
        public const int infuriating_note_1 = 243;
        public const int infuriating_note_2 = 244;
        public const int infuriating_note_3 = 245;
        public const int infuriating_note_4 = 246;
        public const int infuriating_note_5 = 247;
        public const int infuriating_note_6 = 248;
        public const int owl = 249;
        public const int grappling_hook = 250;
        public const int prize_pistol = 251;
        public const int air_strike = 252;
        public const int gungeon_pepper = 253;
        public const int ring_of_chest_friendship = 254;
        public const int ancient_heros_bandana = 255;
        public const int heavy_boots = 256;
        public const int broccoli = 258;
        public const int antibody = 259;
        public const int pink_guon_stone = 260;
        public const int white_guon_stone = 262;
        public const int orange_guon_stone = 263;
        public const int clear_guon_stone = 264;
        public const int old_knights_flask = 267;
        public const int red_guon_stone = 269;
        public const int blue_guon_stone = 270;
        public const int riddle_of_lead = 271;
        public const int iron_coin = 272;
        public const int laser_sight = 273;
        public const int dark_marker = 274;
        public const int flare_gun = 275;
        public const int spice = 276;
        public const int fat_bullets = 277;
        public const int frost_bullets = 278;
        public const int super_hot_watch = 279;
        public const int drum_clip = 280;
        public const int gungeon_blueprint = 281;
        public const int homing_bullets = 284;
        public const int blood_brooch = 285;
        ///<summary>
        ///Spawn name : +1_bullets
        ///</summary>
        public const int _1_bullets = 286;
        public const int backup_gun = 287;
        public const int bouncy_bullets = 288;
        public const int seven_leaf_clover = 289;
        public const int sunglasses = 290;
        public const int meatbun = 291;
        public const int molotov_launcher = 292;
        public const int mimic_tooth_necklace = 293;
        public const int ring_of_mimic_friendship = 294;
        public const int hot_lead = 295;
        public const int yari_launcher_dupe_1 = 296;
        public const int hegemony_credit = 297;
        public const int shock_rounds = 298;
        public const int super_space_turtles_gun = 299;
        public const int dog = 300;
        public const int super_space_turtle = 301;
        public const int bullet_that_can_kill_the_past = 303;
        public const int explosive_rounds = 304;
        public const int old_crest = 305;
        public const int escape_rope = 306;
        public const int wax_wings = 307;
        public const int cluster_mine = 308;
        public const int cloranthy_ring = 309;
        public const int fairy_wings = 310;
        public const int clone = 311;
        public const int blast_helmet = 312;
        public const int monster_blood = 313;
        public const int nanomachines = 314;
        public const int gunboots = 315;
        public const int gnawed_key = 316;
        public const int r2g2 = 318;
        public const int ticket = 320;
        public const int gold_ammolet = 321;
        public const int lodestone_ammolet = 322;
        public const int angry_bullets = 323;
        public const int chaos_ammolet = 325;
        public const int number_2 = 326;
        public const int corsair = 327;
        public const int charge_shot = 328;
        public const int zilla_shotgun = 329;
        public const int the_emperor = 330;
        public const int science_cannon = 331;
        public const int lil_bomber = 332;
        public const int mutation = 333;
        public const int wind_up_gun = 334;
        public const int silencer = 335;
        public const int pitchfork = 336;
        public const int composite_gun = 337;
        public const int gunther = 338;
        public const int mahoguny = 339;
        public const int lower_case_r = 340;
        public const int buzzkill = 341;
        public const int uranium_ammolet = 342;
        public const int copper_ammolet = 343;
        public const int frost_ammolet = 344;
        public const int fightsabre = 345;
        public const int huntsman = 346;
        public const int shotgrub = 347;
        public const int prime_primer = 348;
        public const int planar_lead = 349;
        public const int obsidian_shell_casing = 350;
        public const int arcane_gunpowder = 351;
        public const int shadow_bullets = 352;
        public const int enraging_photo = 353;
        public const int military_training = 354;
        public const int chromesteel_assault_rifle = 355;
        public const int trusty_lockpicks = 356;
        public const int cat_claw = 357;
        public const int railgun = 358;
        public const int compressed_air_tank = 359;
        public const int snakemaker = 360;
        public const int bullet_bore = 362;
        public const int trick_gun = 363;
        public const int heart_of_ice = 364;
        public const int mass_shotgun = 365;
        public const int molotov = 366;
        public const int hunters_journal = 367;
        public const int el_tigre = 368;
        public const int bait_launcher = 369;
        public const int prototype_railgun = 370;
        public const int rc_rocket = 372;
        public const int alpha_bullets = 373;
        public const int omega_bullets = 374;
        public const int easy_reload_bullets = 375;
        public const int brick_breaker = 376;
        public const int excaliber = 377;
        public const int derringer = 378;
        public const int shotgun_full_of_love = 379;
        public const int betrayers_shield = 380;
        public const int triple_crossbow = 381;
        public const int sling = 382;
        public const int flash_ray = 383;
        public const int phoenix = 384;
        public const int hexagun = 385;
        public const int frost_giant = 387;
        public const int cobalt_hammer = 390;
        public const int cell_key = 392;
        public const int anvillain = 393;
        public const int mine_cutter = 394;
        public const int staff_of_firepower = 395;
        public const int table_tech_sight = 396;
        public const int table_tech_money = 397;
        public const int table_tech_rocket = 398;
        public const int table_tech_rage = 399;
        public const int table_tech_blanks = 400;
        public const int gungine = 401;
        public const int snowballer = 402;
        public const int melted_rock = 403;
        public const int siren = 404;
        public const int rattler = 406;
        public const int sixth_chamber = 407;
        public const int busted_television = 409;
        public const int battery_bullets = 410;
        public const int coolant_leak = 411;
        public const int friendship_cookie = 412;
        public const int heros_sword = 413;
        public const int live_ammo = 414;
        public const int replacement_arm = 415;
        public const int balloon = 416;
        public const int blasphemy = 417;
        public const int bug_boots_dupe_1 = 418;
        public const int heart_holster = 421;
        public const int heart_lunchbox = 422;
        public const int heart_locket = 423;
        public const int heart_bottle = 424;
        public const int heart_purse = 425;
        public const int shotga_cola = 426;
        public const int shotgun_coffee = 427;
        public const int bug_boots_dupe_2 = 429;
        public const int liquid_valkyrie = 431;
        public const int jar_of_bees = 432;
        public const int stuffed_star = 433;
        public const int bullet_idol = 434;
        public const int mustache = 435;
        public const int bloodied_scarf = 436;
        public const int muscle_relaxant = 437;
        public const int explosive_decoy = 438;
        public const int bracket_key = 439;
        public const int ruby_bracelet = 440;
        public const int emerald_bracelet = 441;
        public const int badge = 442;
        public const int big_boy = 443;
        public const int trident = 444;
        public const int the_scrambler = 445;
        public const int shield_of_the_maiden = 447;
        public const int boomerang = 448;
        public const int teleporter_prototype = 449;
        public const int armor_synthesizer = 450;
        public const int pig = 451;
        public const int sponge = 452;
        public const int gas_mask = 453;
        public const int hazmat_suit = 454;
        public const int ring_of_triggers = 456;
        public const int armor_of_thorns = 457;
        public const int ring_of_ethereal_form = 458;
        public const int chaff_grenade = 460;
        public const int blank_companions_ring = 461;
        public const int smoke_bomb = 462;
        public const int ring_of_the_resourceful_rat = 463;
        public const int shellegun = 464;
        public const int table_tech_stun = 465;
        public const int green_guon_stone = 466;
        public const int master_round_5 = 467;
        public const int master_round_3 = 468;
        public const int master_round_1 = 469;
        public const int master_round_4 = 470;
        public const int master_round_2 = 471;
        public const int gummy_gun = 472;
        public const int hidden_compartment = 473;
        public const int abyssal_tentacle = 474;
        public const int quad_laser = 475;
        public const int microtransaction_gun = 476;
        public const int origuni = 477;
        public const int banana = 478;
        public const int super_meat_gun = 479;
        public const int makeshift_cannon = 480;
        public const int camera = 481;
        public const int gunzheng = 482;
        public const int tetrominator = 483;
        public const int devolver = 484;
        public const int orange = 485;
        public const int treadnaught_cannon = 486;
        public const int book_of_chest_anatomy = 487;
        public const int ring_of_chest_vampirism = 488;
        public const int gun_soul = 489;
        public const int brick_of_cash = 490;
        public const int wingman = 491;
        public const int wolf = 492;
        public const int briefcase_of_cash = 493;
        public const int galactic_medal_of_valor = 494;
        public const int unity = 495;
        public const int yari_launcher_dupe_2 = 497;
        public const int elder_blank = 499;
        public const int hip_holster = 500;
        public const int yari_launcher_dupe_3 = 501;
        public const int hm_absolution_rockets = 502;
        public const int bullet = 503;
        public const int hyper_light_blaster = 504;
        public const int huntsman_dupe_1 = 505;
        public const int really_special_lute = 506;
        public const int starpew = 507;
        public const int dueling_laser = 508;
        public const int jk47 = 510;
        /// <summary>
        /// Spawn name : 3rd_party_controller
        /// </summary>
        public const int third_party_controller = 511;
        public const int shell = 512;
        public const int poxcannon = 513;
        public const int directional_pad = 514;
        public const int mourning_star = 515;
        public const int triple_gun = 516;
        public const int the_judge_dupe_1 = 517;
        public const int the_judge_dupe_2 = 518;
        public const int combined_rifle = 519;
        public const int balloon_gun = 520;
        public const int chance_bullets = 521;
        public const int stout_bullets = 523;
        public const int bloody_9mm = 524;
        public const int lament_configurum = 525;
        public const int springheel_boots = 526;
        public const int charming_rounds = 527;
        public const int zombie_bullets = 528;
        public const int battle_standard = 529;
        public const int remote_bullets = 530;
        public const int flak_bullets = 531;
        public const int gilded_bullets = 532;
        public const int magic_bullets = 533;
        public const int bow_dupe_1 = 535;
        public const int relodestone = 536;
        public const int vorpal_gun = 537;
        public const int silver_bullets = 538;
        public const int boxing_glove = 539;
        public const int glass_cannon = 540;
        public const int casey = 541;
        public const int strafe_gun = 542;
        public const int the_predator = 543;
        public const int patriot_dupe_1 = 544;
        public const int ac15 = 545;
        public const int windgunner = 546;
        public const int knights_gun = 550;
        public const int crown_of_guns = 551;
        public const int bottle = 558;
        public const int the_fat_line = 562;
        public const int the_exotic = 563;
        public const int full_metal_jacket = 564;
        public const int glass_guon_stone = 565;
        public const int rad_gun = 566;
        public const int roll_bomb = 567;
        public const int helix_bullets = 568;
        public const int chaos_bullets = 569;
        public const int yellow_chamber = 570;
        public const int cursed_bullets = 571;
        public const int chicken_flute = 572;
        public const int chest_teleporter = 573;
        public const int wooden_blasphemy = 574;
        public const int bug_boots_dupe_3 = 575;
        public const int robots_left_hand = 576;
        public const int turbo_gun = 577;
        public const int sprun = 578;
        public const int blank_bullets = 579;
        public const int junkan = 580;
        public const int void_core_cannon = 593;
        public const int moonlight_tiara = 594;
        public const int life_orb = 595;
        public const int teapot = 596;
        public const int mr_accretion_jr = 597;
        public const int stone_dome = 598;
        public const int bubble_blaster = 599;
        public const int partial_ammo = 600;
        public const int big_shotgun = 601;
        public const int gunner = 602;
        public const int lamey_gun = 603;
        public const int slinger = 604;
        public const int loot_bag = 605;
        public const int clown_mask = 607;
        ///<summary>
        ///Spawn name : mutation+neo_tech_yo
        ///</summary>
        public const int mutation_neo_tech_yo = 608;
        ///<summary>
        ///Spawn name : rube_adyne+rubensteins_monster
        ///</summary>
        public const int rube_adyne_rubensteins_monster = 609;
        public const int wood_beam = 610;
        ///<summary>
        ///Spawn name : ak47+island_forme
        ///</summary>
        public const int ak47_island_forme = 611;
        ///<summary>
        ///Spawn name : heroine+wave_beam
        ///</summary>
        public const int heroine_wave_beam = 612;
        ///<summary>
        ///Spawn name : heroine+ice_beam
        ///</summary>
        public const int heroine_ice_beam = 613;
        ///<summary>
        ///Spawn name : heroine+plasma_beam
        ///</summary>
        public const int heroine_plasma_beam = 614;
        ///<summary>
        ///Spawn name : heroine+hyber_beam
        ///</summary>
        public const int heroine_hyber_beam = 615;
        ///<summary>
        ///Spawn name : casey+careful_iteration
        ///</summary>
        public const int casey_careful_iteration = 616;
        ///<summary>
        ///Spawn name : megahand+quick_boomerang
        ///</summary>
        public const int megahand_quick_boomerang = 617;
        ///<summary>
        ///Spawn name : megahand+time_stopper
        ///</summary>
        public const int megahand_time_stopper = 618;
        ///<summary>
        ///Spawn name : megahand+metal_blade
        ///</summary>
        public const int megahand_metal_blade = 619;
        ///<summary>
        ///Spawn name : megahand+leaf_shield
        ///</summary>
        public const int megahand_leaf_shield = 620;
        ///<summary>
        ///Spawn name : megahand+atomic_fire
        ///</summary>
        public const int megahand_atomic_fire = 621;
        ///<summary>
        ///Spawn name : megahand+bubble_lead
        ///</summary>
        public const int megahand_bubble_lead = 622;
        ///<summary>
        ///Spawn name : megahand+air_shooter
        ///</summary>
        public const int megahand_air_shooter = 623;
        ///<summary>
        ///Spawn name : megahand+crash_bomber
        ///</summary>
        public const int megahand_crash_bomber = 624;
        public const int drill = 625;
        public const int elimentaler = 626;
        public const int platinum_bullets = 627;
        public const int bumbullets = 630;
        public const int holey_grail = 631;
        public const int turkey = 632;
        public const int table_tech_shotgun = 633;
        public const int crisis_stone = 634;
        public const int snowballets = 636;
        public const int weird_egg = 637;
        public const int devolver_rounds = 638;
        public const int vorpal_bullets = 640;
        public const int gold_junk = 641;
        public const int daruma = 643;
        public const int portable_table_device = 644;
        public const int turtle_problem = 645;
        public const int chamber_gun = 647;
        public const int lower_case_r_dupe_1 = 648;
        public const int uppercase_r = 649;
        public const int payday_winchester = 650;
        public const int rogue_special_dupe_1 = 651;
        public const int budget_revolver_dupe_1 = 652;
        public const int gun_piece = 654;
        public const int hungry_bullets = 655;
        public const int kruller_glaive = 656;
        public const int flash_ray_dupe_1 = 657;
        public const int proton_backpack_dupe_1 = 658;
        public const int the_exotic_dupe_1 = 659;
        public const int regular_shotgun_dupe_1 = 660;
        public const int orbital_bullets = 661;
        public const int partially_eaten_cheese = 662;
        public const int resourceful_sack = 663;
        public const int baby_good_mimic = 664;
        public const int macho_brace = 665;
        public const int table_tech_heat = 666;
        public const int rat_boots = 667;
        public const int enemy_elimentaler = 668;
        public const int high_dragunfire = 670;
        ///<summary>
        ///Spawn name : gamma_ray+beta_ray
        ///</summary>
        public const int gamma_ray_beta_ray = 671;
        ///<summary>
        ///Spawn name : elephant_gun+the_elephant_in_the_room
        ///</summary>
        public const int elephant_gun_the_elephant_in_the_room = 672;
        ///<summary>
        ///Spawn name : machine_pistol+pistol_machine
        ///</summary>
        public const int machine_pistol_pistol_machine = 673;
        ///<summary>
        ///Spawn name : pea_shooter+pea_cannon
        ///</summary>
        public const int pea_shooter_pea_cannon = 674;
        ///<summary>
        ///Spawn name : dueling_pistol+dualing_pistol
        ///</summary>
        public const int dueling_pistol_dualing_pistol = 675;
        ///<summary>
        ///Spawn name : laser_rifle+laser_light_show
        ///</summary>
        public const int laser_rifle_laser_light_show = 676;
        ///<summary>
        ///Spawn name : dragunfire+kalibreath
        ///</summary>
        public const int dragunfire_kalibreath = 677;
        ///<summary>
        ///Spawn name : blunderbuss+blunderbrace
        ///</summary>
        public const int blunderbuss_blunderbrace = 678;
        ///<summary>
        ///Spawn name : snowballer+snowball_shotgun
        ///</summary>
        public const int snowballer_snowball_shotgun = 679;
        ///<summary>
        ///Spawn name : excaliber+armored_corps
        ///</summary>
        public const int excaliber_armored_corps = 680;
        ///<summary>
        ///Spawn name : 38_special+unknown
        ///</summary>
        public const int thirtyEight_special_unknown = 681;
        ///<summary>
        ///Spawn name : plague_pistol+pandemic_pistol
        ///</summary>
        public const int plague_pistol_pandemic_pistol = 682;
        ///<summary>
        ///Spawn name : thunderclap+alistairs_ladder
        ///</summary>
        public const int thunderclap_alistairs_ladder = 683;
        ///<summary>
        ///Spawn name : m1+m1_multi_tool
        ///</summary>
        public const int m1_m1_multi_tool = 684;
        ///<summary>
        ///Spawn name : thompson+future_gangster
        ///</summary>
        public const int thompson_future_gangster = 685;
        ///<summary>
        ///Spawn name : corsair+black_flag
        ///</summary>
        public const int corsair_black_flag = 686;
        ///<summary>
        ///Spawn name : crestfaller+five_oclock_somewhere
        ///</summary>
        public const int crestfaller_five_oclock_somewhere = 687;
        ///<summary>
        ///Spawn name : banana+fruits_and_vegetables
        ///</summary>
        public const int banana_fruits_and_vegetables = 688;
        ///<summary>
        ///Spawn name : abyssal_tentacle+kalibers_grip
        ///</summary>
        public const int abyssal_tentacle_kalibers_grip = 689;
        ///<summary>
        ///Spawn name : klobbe+klobbering_time
        ///</summary>
        public const int klobbe_klobbering_time = 690;
        ///<summary>
        ///Spawn name : molotov_launcher+special_reserve
        ///</summary>
        public const int molotov_launcher_special_reserve = 691;
        ///<summary>
        ///Spawn name : nail_gun+nailed_it
        ///</summary>
        public const int nail_gun_nailed_it = 692;
        ///<summary>
        ///Spawn name : gunbow+show_across_the_bow
        ///</summary>
        public const int gunbow_show_across_the_bow = 693;
        ///<summary>
        ///Spawn name : big_iron+iron_slug
        ///</summary>
        public const int big_iron_iron_slug = 694;
        ///<summary>
        ///Spawn name : hyper_light_blaster+hard_light
        ///</summary>
        public const int hyper_light_blaster_hard_light = 695;
        ///<summary>
        ///Spawn name : alien_sidearm+chief_master
        ///</summary>
        public const int alien_sidearm_chief_master = 696;
        ///<summary>
        ///Spawn name : shock_rifle+battery_powered
        ///</summary>
        public const int shock_rifle_battery_powered = 697;
        ///<summary>
        ///Spawn name : flame_hand+maximize_spell
        ///</summary>
        public const int flame_hand_maximize_spell = 698;
        ///<summary>
        ///Spawn name : hegemony_rifle+hegemony_special_forces
        ///</summary>
        public const int hegemony_rifle_hegemony_special_forces = 699;
        ///<summary>
        ///Spawn name : cactus+cactus_flower
        ///</summary>
        public const int cactus_cactus_flower = 700;
        ///<summary>
        ///Spawn name : luxin_cannon+noxin_cannon
        ///</summary>
        public const int luxin_cannon_noxin_cannon = 701;
        ///<summary>
        ///Spawn name : face_melter+alternative_rock
        ///</summary>
        public const int face_melter_alternative_rock = 702;
        ///<summary>
        ///Spawn name : bee_hive+apiary
        ///</summary>
        public const int bee_hive_apiary = 703;
        ///<summary>
        ///Spawn name : trashcannon+recycling_bin
        ///</summary>
        public const int trashcannon_recycling_bin = 704;
        ///<summary>
        ///Spawn name : flash_ray+savior_of_the_universe
        ///</summary>
        public const int flash_ray_savior_of_the_universe = 705;
        ///<summary>
        ///Spawn name : flare_gun+firing_with_flair
        ///</summary>
        public const int flare_gun_firing_with_flair = 706;
        ///<summary>
        ///Spawn name : vulcan_cannon+not_quite_as_mini
        ///</summary>
        public const int vulcan_cannon_not_quite_as_mini = 707;
        ///<summary>
        ///Spawn name : helix+double_double_helix
        ///</summary>
        public const int helix_double_double_helix = 708;
        ///<summary>
        ///Spawn name : barrel+like_shooting_fish
        ///</summary>
        public const int barrel_like_shooting_fish = 709;
        ///<summary>
        ///Spawn name : freeze_ray+ice_cap
        ///</summary>
        public const int freeze_ray_ice_cap = 710;
        ///<summary>
        ///Spawn name : light_gun+peripheral_vision
        ///</summary>
        public const int light_gun_peripheral_vision = 711;
        ///<summary>
        ///Spawn name : raiden_coil+raiden
        ///</summary>
        public const int raiden_coil_raiden = 712;
        ///<summary>
        ///Spawn name : moonscraper+double_moon_7
        ///</summary>
        public const int moonscraper_double_moon_7 = 713;
        ///<summary>
        ///Spawn name : laser_lotus+lotus_bloom
        ///</summary>
        public const int laser_lotus_lotus_bloom = 714;
        ///<summary>
        ///Spawn name : h4mmer+hammer_and_nail
        ///</summary>
        public const int h4mmer_hammer_and_nail = 715;
        ///<summary>
        ///Spawn name : awp+arctic_warfare
        ///</summary>
        public const int awp_arctic_warfare = 716;
        ///<summary>
        ///Spawn name : bullet_bore+cerebral_bros
        ///</summary>
        public const int bullet_bore_cerebral_bros = 717;
        ///<summary>
        ///Spawn name : polaris+square_brace
        ///</summary>
        public const int polaris_square_brace = 718;
        ///<summary>
        ///Spawn name : lil_bomber+king_bomber
        ///</summary>
        public const int lil_bomber_king_bomber = 719;
        ///<summary>
        ///Spawn name : proton_backpack+electron_pack
        ///</summary>
        public const int proton_backpack_electron_pack = 720;
        ///<summary>
        ///Spawn name : jolter+heavy_jolt
        ///</summary>
        public const int jolter_heavy_jolt = 721;
        ///<summary>
        ///Spawn name : pitchfork+pitch_perfect
        ///</summary>
        public const int pitchfork_pitch_perfect = 722;
        ///<summary>
        ///Spawn name : com4nd0+commammo_belt
        ///</summary>
        public const int com4nd0_commammo_belt = 723;
        ///<summary>
        ///Spawn name : hegemony_carbine+ruby_carbine
        ///</summary>
        public const int hegemony_carbine_ruby_carbine = 724;
        ///<summary>
        ///Spawn name : tear_jerker+unknown
        ///</summary>
        public const int tear_jerker_unknown = 725;
        ///<summary>
        ///Spawn name : akey47+akey_breaky
        ///</summary>
        public const int akey47_akey_breaky = 726;
        public const int rat_key = 727;
        public const int unknown_7 = 728;
        public const int unknown_8 = 729;
        public const int unknown_9 = 730;
        public const int unknown_10 = 731;
        public const int gunderfury_lv10 = 732;
        public const int unknown_11 = 733;
        public const int mimic_gun = 734;
        public const int serpent = 735;
        ///<summary>
        ///Spawn name : phoenix+phoenix_up
        ///</summary>
        public const int phoenix_phoenix_up = 736;
        ///<summary>
        ///Spawn name : betrayers_shield+betrayers_lies
        ///</summary>
        public const int betrayers_shield_betrayers_lies = 737;
        ///<summary>
        ///Spawn name : lower_case_r+unknown
        ///</summary>
        public const int lower_case_r_unknown = 738;
        ///<summary>
        ///Spawn name : gungeon_ant+great_queen_ant
        ///</summary>
        public const int gungeon_ant_great_queen_ant = 739;
        ///<summary>
        ///Spawn name : buzzkill+not_so_sawed_off
        ///</summary>
        public const int buzzkill_not_so_sawed_off = 740;
        ///<summary>
        ///Spawn name : tear_jerker+wrath_of_the_blam
        ///</summary>
        public const int tear_jerker_wrath_of_the_blam = 741;
        ///<summary>
        ///Spawn name : alien_engine+contrail
        ///</summary>
        public const int alien_engine_contrail = 742;
        ///<summary>
        ///Spawn name : rad_gun+kung_fu_hippie_rappin_surfer
        ///</summary>
        public const int rad_gun_kung_fu_hippie_rappin_surfer = 743;
        ///<summary>
        ///Spawn name : origuni+parchmental
        ///</summary>
        public const int origuni_parchmental = 744;
        ///<summary>
        ///Spawn name : ice_breaker+gunderlord
        ///</summary>
        public const int ice_breaker_gunderlord = 745;
        ///<summary>
        ///Spawn name : high_dragunfire+unknown
        ///</summary>
        public const int high_dragunfire_unknown = 747;
        public const int sunlight_javelin = 748;
        ///<summary>
        ///Spawn name : shotbow+second_accident
        ///</summary>
        public const int shotbow_second_accident = 749;
        ///<summary>
        ///Spawn name : dungeon_eagle+dont_hoot_the_messenger
        ///</summary>
        public const int dungeon_eagle_dont_hoot_the_messenger = 750;
        ///<summary>
        ///Spawn name : magnum+unknown_synergy
        ///</summary>
        public const int magnum_unknown_synergy = 751;
        ///<summary>
        ///Spawn name : smileys_revolver+unknown_synergy_1
        ///</summary>
        public const int smileys_revolver_unknown_synergy_1 = 752;
        ///<summary>
        ///Spawn name : smileys_revolver+unknown_synergy_2
        ///</summary>
        public const int smileys_revolver_unknown_synergy_2 = 753;
        ///<summary>
        ///Spawn name : smileys_revolver+unknown_synergy_3
        ///</summary>
        public const int smileys_revolver_unknown_synergy_3 = 754;
        public const int evolver = 755;
        ///<summary>
        ///Spawn name : shell+unknown_synergy_1
        ///</summary>
        public const int shell_unknown_synergy_1 = 756;
        ///<summary>
        ///Spawn name : shell+unknown_synergy_2
        ///</summary>
        public const int shell_unknown_synergy_2 = 757;
        ///<summary>
        ///Spawn name : shell+unknown_synergy_3
        ///</summary>
        public const int shell_unknown_synergy_3 = 758;
        ///<summary>
        ///Spawn name : shell+unknown_synergy_4
        ///</summary>
        public const int shell_unknown_synergy_4 = 759;
        ///<summary>
        ///Spawn name : shell+unknown_synergy_5
        ///</summary>
        public const int shell_unknown_synergy_5 = 760;
        public const int high_kaliber = 761;
        public const int finished_gun = 762;
        ///<summary>
        ///Spawn name : regular_shotgun+unknown_synergy_1
        ///</summary>
        public const int regular_shotgun_unknown_synergy_1 = 763;
        ///<summary>
        ///Spawn name : unfinished_gun+unknown_synergy_2
        ///</summary>
        public const int unfinished_gun_unknown_synergy_2 = 806;
        ///<summary>
        ///Spawn name : unfinished_gun+unknown_synergy_3
        ///</summary>
        public const int unfinished_gun_unknown_synergy_3 = 807;
        ///<summary>
        ///Spawn name : the_exotic+unknown_synergy_1
        ///</summary>
        public const int the_exotic_unknown_synergy_1 = 808;
        public const int marine_sidearm_alt = 809;
        public const int rusty_sidearm_alt = 810;
        public const int dart_gun_alt = 811;
        public const int robots_right_hand_alt = 812;
        public const int blasphemy_alt = 813;
        public const int magazine_rack = 814;
        public const int lichs_eye_bullets = 815;
        public const int trank_gun_dupe_1 = 816;
        public const int cat_bullet_king_throne = 817;
        public const int baby_good_shelleton = 818;
        ///<summary>
        ///Spawn name : glass_cannon+steel_skin
        ///</summary>
        public const int glass_cannon_steel_skin = 819;
        public const int shadow_clone = 820;
        public const int scouter = 821;
        public const int katana_bullets = 822;
        public const int wood_beam_dupe_1 = 823;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #endregion
    }
}
