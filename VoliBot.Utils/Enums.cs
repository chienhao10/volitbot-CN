using System;

namespace VoliBot.Utils
{
	internal class Enums
	{
		public static object[] champions = new object[]
		{
			"AATROX",
			"AHRI",
			"AKALI",
			"ALISTAR",
			"AMUMU",
			"ANIVIA",
			"ANNIE",
			"ASHE",
			"AZIR",
			"BLITZCRANK",
			"BARD",
			"BRAND",
			"BRAUM",
			"CAITLYN",
			"CASSIOPEIA",
			"CHOGATH",
			"CORKI",
			"DARIUS",
			"DIANA",
			"MUNDO",
			"DRAVEN",
			"EKKO",
			"ELISE",
			"EVELYNN",
			"EZREAL",
			"FIDDLESTICKS",
			"FIORA",
			"FIZZ",
			"GALIO",
			"GANGPLANK",
			"GAREN",
			"GNAR",
			"GRAGAS",
			"GRAVES",
			"HECARIM",
			"HEIMERDIGER",
			"IRELIA",
			"JANNA",
			"JARVAN",
			"JAX",
			"JAYCE",
			"JINX",
			"KALISTA",
			"KARMA",
			"KARTHUS",
			"KASSADIN",
			"KATARINA",
			"KAYLE",
			"KENNEN",
			"KHAZIX",
			"KOGMAW",
			"LEBLANC",
			"LEESIN",
			"LEONA",
			"LISSANDRA",
			"LUCIAN",
			"LULU",
			"LUX",
			"MALPHITE",
			"MALZAHAR",
			"MAOKAI",
			"MASTERYI",
			"MISSFORTUNE",
			"MORDEKAISER",
			"MORGANA",
			"NAMI",
			"NASUS",
			"NAUTILUS",
			"NIDALEE",
			"NOCTURNE",
			"NUNU",
			"OLAF",
			"ORIANNA",
			"PANTHEON",
			"POPPY",
			"QUINN",
			"REKSAI",
			"RAMMUS",
			"RENEKTON",
			"RENGAR",
			"RIVEN",
			"RUMBLE",
			"RYZE",
			"SEJUANI",
			"SHACO",
			"SHEN",
			"SHYVANA",
			"SINGED",
			"SION",
			"SIVIR",
			"SKARNER",
			"SONA",
			"SORAKA",
			"SWAIN",
			"SYNDRA",
			"TALON",
			"TARIC",
			"TEEMO",
			"THRESH",
			"TRISTANA",
			"TRUNDLE",
			"TRYNDAMERE",
			"TWISTEDFATE",
			"TWITCH",
			"UDYR",
			"URGOT",
			"VARUS",
			"VAYNE",
			"VEIGAR",
			"VELKOZ",
			"VI",
			"VIKTOR",
			"VLADIMIR",
			"VOLIBEAR",
			"WARWICK",
			"WUKONG",
			"XERATH",
			"XINZHAO",
			"YASUO",
			"YORICK",
			"ZAC",
			"ZED",
			"ZIGGS",
			"ZILEAN",
			"ZYRA"
		};

		public static object[] queues = new object[]
		{
			"NORMAL_5x5",
			"NORMAL_3x3",
			"INTRO_BOT",
			"BEGINNER_BOT",
			"MEDIUM_BOT",
			"ARAM"
		};

		public static object[] regions = new object[]
		{
			"NA",
			"EUW",
			"EUNE",
			"OCE",
			"LAN",
			"LAS",
			"BR",
			"TR",
			"RU",
			"KR"
		};

		public static object[] spells = new object[]
		{
			"BARRIER",
			"CLAIRVOYANCE",
			"CLARITY",
			"CLEANSE",
			"EXHAUST",
			"FLASH",
			"GARRISON",
			"GHOST",
			"HEAL",
			"IGNITE",
			"SMITE",
			"TELEPORT"
		};

		public static string championToString(int id)
		{
			if (id <= 254)
			{
				switch (id)
				{
				case 1:
					return "ANNIE";
				case 2:
					return "OLAF";
				case 3:
					return "GALIO";
				case 4:
					return "TWISTED-FATE";
				case 5:
					return "XIN-ZHAO";
				case 6:
					return "URGOT";
				case 7:
					return "LEBLANC";
				case 8:
					return "VLADIMIR";
				case 9:
					return "FIDDLESTICKS";
				case 10:
					return "KAYLE";
				case 11:
					return "MASTER-YI";
				case 12:
					return "ALISTAR";
				case 13:
					return "RYZE";
				case 14:
					return "SION";
				case 15:
					return "SIVIR";
				case 16:
					return "SORAKA";
				case 17:
					return "TEEMO";
				case 18:
					return "TRISTANA";
				case 19:
					return "WARWICK";
				case 20:
					return "NUNU";
				case 21:
					return "MISS-FORTUNE";
				case 22:
					return "ASHE";
				case 23:
					return "TRYNDAMERE";
				case 24:
					return "JAX";
				case 25:
					return "MORGANA";
				case 26:
					return "ZILEAN";
				case 27:
					return "SINGED";
				case 28:
					return "EVELYNN";
				case 29:
					return "TWITCH";
				case 30:
					return "KARTHUS";
				case 31:
					return "CHOGATH";
				case 32:
					return "AMUMU";
				case 33:
					return "RAMMUS";
				case 34:
					return "ANIVIA";
				case 35:
					return "SHACO";
				case 36:
					return "DR-MUNDO";
				case 37:
					return "SONA";
				case 38:
					return "KASSADIN";
				case 39:
					return "IRELIA";
				case 40:
					return "JANNA";
				case 41:
					return "GANGPLANK";
				case 42:
					return "CORKI";
				case 43:
					return "KARMA";
				case 44:
					return "TARIC";
				case 45:
					return "VEIGAR";
				case 46:
				case 47:
				case 49:
				case 52:
				case 65:
				case 66:
				case 70:
				case 71:
				case 73:
				case 87:
				case 88:
				case 93:
				case 94:
				case 95:
				case 97:
				case 100:
				case 108:
				case 109:
				case 116:
				case 118:
				case 123:
				case 124:
				case 125:
				case 128:
				case 129:
				case 130:
				case 132:
				case 135:
				case 136:
				case 137:
				case 138:
				case 139:
				case 140:
				case 141:
				case 142:
				case 144:
				case 145:
				case 146:
				case 147:
				case 148:
				case 149:
				case 151:
				case 152:
				case 153:
				case 155:
				case 156:
				case 158:
				case 159:
				case 160:
				case 162:
				case 163:
				case 164:
				case 165:
				case 166:
				case 167:
				case 168:
				case 169:
				case 170:
				case 171:
				case 172:
				case 173:
				case 174:
				case 175:
				case 176:
				case 177:
				case 178:
				case 179:
				case 180:
				case 181:
				case 182:
				case 183:
				case 184:
				case 185:
				case 186:
				case 187:
				case 188:
				case 189:
				case 190:
				case 191:
				case 192:
				case 193:
				case 194:
				case 195:
				case 196:
				case 197:
				case 198:
				case 199:
				case 200:
				case 202:
				case 203:
				case 204:
				case 205:
				case 206:
				case 207:
				case 208:
				case 209:
				case 210:
				case 211:
				case 212:
				case 213:
				case 214:
				case 215:
				case 216:
				case 217:
				case 218:
				case 219:
				case 220:
				case 221:
					break;
				case 48:
					return "TRUNDLE";
				case 50:
					return "SWAIN";
				case 51:
					return "CAITLYN";
				case 53:
					return "BLITZCRANK";
				case 54:
					return "MALPHITE";
				case 55:
					return "KATARINA";
				case 56:
					return "NOCTURNE";
				case 57:
					return "MAOKAI";
				case 58:
					return "RENEKTON";
				case 59:
					return "JARVAN-IV";
				case 60:
					return "ELISE";
				case 61:
					return "ORIANNA";
				case 62:
					return "WUKONG";
				case 63:
					return "BRAND";
				case 64:
					return "LEE-SIN";
				case 67:
					return "VAYNE";
				case 68:
					return "RUMBLE";
				case 69:
					return "CASSIOPEIA";
				case 72:
					return "SKARNER";
				case 74:
					return "HEIMERDINGER";
				case 75:
					return "NASUS";
				case 76:
					return "NIDALEE";
				case 77:
					return "UDYR";
				case 78:
					return "POPPY";
				case 79:
					return "GRAGAS";
				case 80:
					return "PANTHEON";
				case 81:
					return "EZREAL";
				case 82:
					return "MORDEKAISER";
				case 83:
					return "YORICK";
				case 84:
					return "AKALI";
				case 85:
					return "KENNEN";
				case 86:
					return "GAREN";
				case 89:
					return "LEONA";
				case 90:
					return "MALZAHAR";
				case 91:
					return "TALON";
				case 92:
					return "RIVEN";
				case 96:
					return "KOGMAW";
				case 98:
					return "SHEN";
				case 99:
					return "LUX";
				case 101:
					return "XERATH";
				case 102:
					return "SHYVANA";
				case 103:
					return "AHRI";
				case 104:
					return "GRAVES";
				case 105:
					return "FIZZ";
				case 106:
					return "VOLIBEAR";
				case 107:
					return "RENGAR";
				case 110:
					return "VARUS";
				case 111:
					return "NAUTILUS";
				case 112:
					return "VIKTOR";
				case 113:
					return "SEJUANI";
				case 114:
					return "FIORA";
				case 115:
					return "ZIGGS";
				case 117:
					return "LULU";
				case 119:
					return "DRAVEN";
				case 120:
					return "HECARIM";
				case 121:
					return "KHAZIX";
				case 122:
					return "DARIUS";
				case 126:
					return "JAYCE";
				case 127:
					return "LISSANDRA";
				case 131:
					return "DIANA";
				case 133:
					return "QUINN";
				case 134:
					return "SYNDRA";
				case 143:
					return "ZYRA";
				case 150:
					return "GNAR";
				case 154:
					return "ZAC";
				case 157:
					return "YASUO";
				case 161:
					return "VELKOZ";
				case 201:
					return "BRAUM";
				case 222:
					return "JINX";
				default:
					switch (id)
					{
					case 236:
						return "LUCIAN";
					case 237:
						break;
					case 238:
						return "ZED";
					default:
						if (id == 254)
						{
							return "VI";
						}
						break;
					}
					break;
				}
			}
			else if (id <= 412)
			{
				switch (id)
				{
				case 266:
					return "AATROX";
				case 267:
					return "NAMI";
				case 268:
					return "AZIR";
				default:
					if (id == 412)
					{
						return "THRESH";
					}
					break;
				}
			}
			else
			{
				if (id == 421)
				{
					return "REKSAI";
				}
				if (id == 429)
				{
					return "KALISTA";
				}
			}
			return "UNKNOWN";
		}

		public static int championToId(string name)
		{
			switch (name)
			{
			case "AATROX":
				return 266;
			case "AHRI":
				return 103;
			case "AKALI":
				return 84;
			case "ALISTAR":
				return 12;
			case "AMUMU":
				return 32;
			case "ANIVIA":
				return 34;
			case "ANNIE":
				return 1;
			case "ASHE":
				return 22;
			case "AZIR":
				return 268;
			case "BLITZCRANK":
				return 53;
			case "BRAND":
				return 63;
			case "BRAUM":
				return 201;
			case "CAITLYN":
				return 51;
			case "CASSIOPEIA":
				return 69;
			case "CHOGATH":
				return 31;
			case "CORKI":
				return 42;
			case "DARIUS":
				return 122;
			case "DIANA":
				return 131;
			case "MUNDO":
				return 36;
			case "DRAVEN":
				return 119;
			case "ELISE":
				return 60;
			case "EVELYNN":
				return 28;
			case "EZREAL":
				return 81;
			case "FIDDLESTICKS":
				return 9;
			case "FIORA":
				return 114;
			case "FIZZ":
				return 105;
			case "GALIO":
				return 3;
			case "GANGPLANK":
				return 41;
			case "GAREN":
				return 86;
			case "GNAR":
				return 150;
			case "GRAGAS":
				return 79;
			case "GRAVES":
				return 104;
			case "HECARIM":
				return 120;
			case "HEIMERDIGER":
				return 74;
			case "IRELIA":
				return 39;
			case "JANNA":
				return 40;
			case "JARVAN":
				return 59;
			case "JAX":
				return 24;
			case "JAYCE":
				return 126;
			case "JINX":
				return 222;
			case "KALISTA":
				return 429;
			case "KARMA":
				return 43;
			case "KARTHUS":
				return 30;
			case "KASSADIN":
				return 38;
			case "KATARINA":
				return 55;
			case "KAYLE":
				return 10;
			case "KENNEN":
				return 85;
			case "KHAZIX":
				return 121;
			case "KOGMAW":
				return 96;
			case "LEBLANC":
				return 7;
			case "LEESIN":
				return 64;
			case "LEONA":
				return 89;
			case "LISSANDRA":
				return 127;
			case "LUCIAN":
				return 236;
			case "LULU":
				return 117;
			case "LUX":
				return 99;
			case "MALPHITE":
				return 54;
			case "MALZAHAR":
				return 90;
			case "MAOKAI":
				return 57;
			case "MASTERYI":
				return 11;
			case "MISSFORTUNE":
				return 21;
			case "MORDEKAISER":
				return 82;
			case "MORGANA":
				return 25;
			case "NAMI":
				return 267;
			case "NASUS":
				return 75;
			case "NAUTILUS":
				return 111;
			case "NIDALEE":
				return 76;
			case "NOCTURNE":
				return 56;
			case "NUNU":
				return 20;
			case "OLAF":
				return 2;
			case "ORIANNA":
				return 61;
			case "PANTHEON":
				return 80;
			case "POPPY":
				return 78;
			case "QUINN":
				return 133;
			case "REKSAI":
				return 421;
			case "RAMMUS":
				return 33;
			case "RENEKTON":
				return 58;
			case "RENGAR":
				return 107;
			case "RIVEN":
				return 92;
			case "RUMBLE":
				return 68;
			case "RYZE":
				return 13;
			case "SEJUANI":
				return 113;
			case "SHACO":
				return 35;
			case "SHEN":
				return 98;
			case "SHYVANA":
				return 102;
			case "SINGED":
				return 27;
			case "SION":
				return 14;
			case "SIVIR":
				return 15;
			case "SKARNER":
				return 72;
			case "SONA":
				return 37;
			case "SORAKA":
				return 16;
			case "SWAIN":
				return 50;
			case "SYNDRA":
				return 134;
			case "TALON":
				return 91;
			case "TARIC":
				return 44;
			case "TEEMO":
				return 17;
			case "THRESH":
				return 412;
			case "TRISTANA":
				return 18;
			case "TRUNDLE":
				return 48;
			case "TRYNDAMERE":
				return 23;
			case "TWISTEDFATE":
				return 4;
			case "TWITCH":
				return 29;
			case "UDYR":
				return 77;
			case "URGOT":
				return 6;
			case "VARUS":
				return 110;
			case "VAYNE":
				return 67;
			case "VEIGAR":
				return 45;
			case "VELKOZ":
				return 161;
			case "VI":
				return 254;
			case "VIKTOR":
				return 112;
			case "VLADIMIR":
				return 8;
			case "VOLIBEAR":
				return 106;
			case "WARWICK":
				return 19;
			case "WUKONG":
				return 62;
			case "XERATH":
				return 101;
			case "XINZHAO":
				return 5;
			case "YASUO":
				return 157;
			case "YORICK":
				return 83;
			case "ZAC":
				return 154;
			case "ZED":
				return 238;
			case "ZIGGS":
				return 115;
			case "ZILEAN":
				return 26;
			case "ZYRA":
				return 143;
			}
			return 0;
		}

		public static int spellToId(string name)
		{
			switch (name)
			{
			case "BARRIER":
				return 21;
			case "CLAIRVOYANCE":
				return 2;
			case "CLARITY":
				return 13;
			case "CLEANSE":
				return 1;
			case "EXHAUST":
				return 3;
			case "FLASH":
				return 4;
			case "GARRISON":
				return 17;
			case "GHOST":
				return 6;
			case "HEAL":
				return 7;
			case "IGNITE":
				return 14;
			case "SMITE":
				return 11;
			case "TELEPORT":
				return 12;
			}
			return 0;
		}
	}
}
