using System;

namespace LoLLauncher
{
	public enum RegioN
	{
		[LocaleValue("en_US"), LoginQueueValue("https://lq.na2.lol.riotgames.com/"), ServerValue("prod.na2.lol.riotgames.com"), UseGarenaValue(false)]
		NA,
		[LocaleValue("en_GB"), LoginQueueValue("https://lq.euw1.lol.riotgames.com/"), ServerValue("prod.euw1.lol.riotgames.com"), UseGarenaValue(false)]
		EUW,
		[LocaleValue("en_GB"), LoginQueueValue("https://lq.eun1.lol.riotgames.com/"), ServerValue("prod.eun1.lol.riotgames.com"), UseGarenaValue(false)]
		EUN,
		[LocaleValue("ko_KR"), LoginQueueValue("https://lq.kr.lol.riotgames.com/"), ServerValue("prod.kr.lol.riotgames.com"), UseGarenaValue(false)]
		KR,
		[LocaleValue("pt_BR"), LoginQueueValue("https://lq.br.lol.riotgames.com/"), ServerValue("prod.br.lol.riotgames.com"), UseGarenaValue(false)]
		BR,
		[LocaleValue("pt_BR"), LoginQueueValue("https://lq.tr.lol.riotgames.com/"), ServerValue("prod.tr.lol.riotgames.com"), UseGarenaValue(false)]
		TR,
		[LocaleValue("en_US"), LoginQueueValue("https://lq.ru.lol.riotgames.com/"), ServerValue("prod.ru.lol.riotgames.com"), UseGarenaValue(false)]
		RU,
		[LocaleValue("es_MX"), LoginQueueValue("https://lq.la1.lol.riotgames.com/"), ServerValue("prod.la1.lol.riotgames.com"), UseGarenaValue(false)]
		LA1,
		[LocaleValue("es_MX"), LoginQueueValue("https://lq.la2.lol.riotgames.com/"), ServerValue("prod.la2.lol.riotgames.com"), UseGarenaValue(false)]
		LA2,
		[LocaleValue("en_US"), LoginQueueValue("https://lq.pbe1.lol.riotgames.com/"), ServerValue("prod.pbe1.lol.riotgames.com"), UseGarenaValue(false)]
		PBE,
		[LocaleValue("en_US"), LoginQueueValue("https://lq.lol.garenanow.com/"), ServerValue("prod.lol.garenanow.com"), UseGarenaValue(true)]
		SG,
		[LocaleValue("en_US"), LoginQueueValue("https://lq.lol.garenanow.com/"), ServerValue("prod.lol.garenanow.com"), UseGarenaValue(true)]
		MY,
		[LocaleValue("en_US"), LoginQueueValue("https://lq.lol.garenanow.com/"), ServerValue("prod.lol.garenanow.com"), UseGarenaValue(true)]
		SGMY,
		[LocaleValue("en_US"), LoginQueueValue("https://loginqueuetw.lol.garenanow.com/"), ServerValue("prodtw.lol.garenanow.com"), UseGarenaValue(true)]
		TW,
		[LocaleValue("en_US"), LoginQueueValue("https://lqth.lol.garenanow.com/"), ServerValue("prodth.lol.garenanow.com"), UseGarenaValue(true)]
		TH,
		[LocaleValue("en_US"), LoginQueueValue("https://lqph.lol.garenanow.com/"), ServerValue("prodph.lol.garenanow.com"), UseGarenaValue(true)]
		PH,
		[LocaleValue("en_US"), LoginQueueValue("https://lqvn.lol.garenanow.com/"), ServerValue("prodvn.lol.garenanow.com"), UseGarenaValue(true)]
		VN,
		[LocaleValue("en_US"), LoginQueueValue("https://lq.oc1.lol.riotgames.com/"), ServerValue("prod.oc1.lol.riotgames.com"), UseGarenaValue(false)]
		OCE,
		[LocaleValue("en_US"), LoginQueueValue("https://lq.cs.lol.riotgames.com/"), ServerValue("prod.cs.lol.riotgames.com"), UseGarenaValue(false)]
		CS
	}
}
