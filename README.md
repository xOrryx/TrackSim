# TrackSim

*Autor - Petr Šopák*

## Úvod
TrackSim vznikl pro vytváření tratí pro projekt do předmětu BPC-PRP (VUT Brno). Aplikace je vytvořena pomocí Unity herní platformy.

Aplikaci lze stahnout **[ZDE](https://drive.google.com/file/d/1BBbptLxssqmlcz7bytOO-2H97Z4qp4ye/view?usp=sharing)**

Příklad .yaml souboru lze stahnout **[ZDE]()**

**Samotná aplikace je podporována pro _Windows_.** Důvod je vysvětlen v realizaci níže.

## Popis realizace

Vzhled aplikaci je vyzobrazen na obrázku (obr.1.), kde lze vidět všechny objekty uživatelského rozhraní (UI) a primitivního objektu *Plane*. UI představuje výběr či nastavování objektu Plane a samotné vykreslování čáry dle požadavků uživatele. Plane je mesh struktura vytvořena přímo Unity, která nabízí pár užitečných funkcí pro zjednodušení realizace.

<div align = "center">
<img width="550" height="400" src="/uploads/1695c35480e635f1b0c9bfe174f6357b/char1.PNG">

Obr.1.: Vzhled aplikace TrackSim
</div>

### Kolize a paprsky

Samotným ideem bylo umožnit uživatele si naklikat body na rovinu, díky které by se došlo k optimalizovanému řešení, neboli by uživatel naklikával vždy počáteční a koncové body úseček anebo oblouků. Toto by zejména u úseček snížilo výpis bodů na dva oproti bežného výpočtu bodů zavislé na vzdálenosti výpisu bodů o parametr *ratio*.

Byly použité *paprsky* (rays) pro naklikávání bodů na plane, kdy jsou při každém kliknutí na *Canvas* (rovina pro pokládání objektů uživatelského rozhraní - je zde připojena k obrazu hlavní kamery *main*) je přepočítaná do *World* souřadného systému. Následně paprsky vrací kolize prvního (RayCast) anebo všech objektů (RayCastAll), kterými prochází, ale současně musí objekty tuto vlastnost kolize vlastnit. Proto k Planu byl připojen *komponent MeshCollidor* (komponenty jsou Assety objektu, které jsou k objektům připojeny a dodávájí jim přislušnou funkci), který vytvoří Collider mezi vrstvou mesh a meshBounds pro zachytávání událostí (Events). Tento proces Lze vidět na obrázku (obr.2.). Jak bylo předem řečeno, že Plane dává funkce, které ostatní primitivní objekty nedodávají a to metodu *ClosestPointOnPlane*, která vrací nejbližší bod v okolí projití paprsku Planem. Tímto je získáná přesnost během pozdějšího výpočtům mezi body například pro výpočet oblouku.

<div align = "center">
<img width="550" height="400" src="/uploads/1695c35480e635f1b0c9bfe174f6357b/char1.PNG">

Obr.2.: Použití paprsků a kolize pro získávaní bodů
</div>

### Výpočet oblouku (curve) a vedlejších čar (Triline)

Jak je vidět na obrázku (obr.2.), úsečky nejsou problém zrealizovat, ale pro oblouky by uživatel musel naklikávat *n* počet bodů, proto byla použita Bezierova křivka. Je potřeba naklikat tři body - počáteční, střední a koncový bod, kde střední bod určuje zakřivení oblouku a není dále součástí exportovaných bodů. Díky této křivky jsou získány body vzdálené o parametr *r = 0,1* (lze ji změnit v programu) mezi počátečním a koncovým bodem. 

Vedlejší čáry jsou realizováné za pomocí klonováním naklikaných bodů a posunutí je o po ose X anebo ose Y dle směru vektoru mezi počatečním a koncovým bodem. Bohužel byl využit *Vector3.Angle()*, který vrací jenom kladné úhly (180-0-180), proto byl vytvářen virtuální souřadný systém, který zařazoval vektory do kvadrantů. Vektor byl vždy tedy posunutý do počátku systému a směr udával kvadrant, který pak vyhodnotil stranu vedlejší čáry a její posunutí. Tento způsob zajistil správné posunutí bodů po jedné ose, ale i v situaci, kdy vektor měl +-45° sklon od vodorovné osy. V této situaci musí být body posunuty po obouch osách. 

Vedlejší oblouk byl vytvořen stejným způsobem, kdy všechny tři potřebné body byly posunuty a pak zavolaná funkce pro realizaci oblouku. Během přednášek bylo řečeno, že tloušťka mezery není determinována, ale přednášející tipoval 2cm. Toušťky všech čar jsou 2cm. Pro čáry a oblouky je nastavena vzdálenost bodu *d = 4 [cm]*.

### Vizualizace čar a bodů

Hlavní trasa vždy má počátek v bodě [0,0,0] (dle pravidel soutěže) a následně jsou k nim přídávány kolizní body. Počáteční a naklikané body jsou vizuálně naznačeny primitivním objektem *krychle (Cube)*. Body jsou pospojeny pomocí _jedné_ čáry pomocí *LineRenderer* komponenty přidané pevně na *empty Objekt*. LineRenderer zajišťuje vytváření komplexních čar pomocí zadáním počtu a pozice bodů. Vizualizace čar lze vidět na obázku (obr.3.)

<div align = "center">
<img width="550" height="400" src="/uploads/1695c35480e635f1b0c9bfe174f6357b/char1.PNG">

Obr.3.: Vizualizace čar a bodů
</div>

### Rastr

Pro zjednodušení naklikávání bodů uživatelem byl vytvořen *Rastr*, který lze vidět na obrázku (obr.1.). Bylo k tomu použit znovu objekt s LineRendererem. Následně byly vytvořeny čáry dle velikosti Planu. Bohužel Plane nenabízí *Transform* komponent (nabízí velikost, pozici apod.), tedy velikost musela být odměřena manuálně. Čáry byly následně klonovány a posunuty o velikost *d*, kde určuje velikost mezery [cm] a je určen uživatelem, či případně rotovány.

### Export

Unity nenabízí *standalone File browser*, který by fungoval v Runtime aplikaci. Proto byl zde použita knihovna od **[SrejonKhan](https://github.com/SrejonKhan/AnotherFileBrowser)**. Bohužel tato knihovna nepodporuje Linux, ale jenom Windows a iOS, proto je tato aplikace omezena na Windows (pro iOS nebyla odzkoušena), ale díky této knihovny zase lze vybrat místo uložení souboru, filtrace a přepis výsledného formátu souboru.

*YAML soubor* byl vytvořen za pomocí textového souboru, do kterého jsou body uloženy do vytvořené šablony, aby měl strukturu jako zdrojový soubor přiložený na počátku semestru. Body jsou ukládány postupně, kdy prvně jsou uloženy body hlavní trasy a následně body tvořící vedlejší čáry.

## TODO

Pro zjednodušení práce bylo by vhodné implementovat **odstranění posledního bodu či reset trasy**, protože při chybného zakliknutí anebo po rozmyslení je potřeba resetovat celou aplikaci. Ostatní členové týmu toto nepotřebovali, tak to nebylo implementováno. 

