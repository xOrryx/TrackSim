# TrackSim

*Autor - Petr Šopák (221022)*

## Úvod
TrackSim vznikl pro vytváření tratí pro projekt do předmětu BPC-PRP (VUT Brno). Aplikace je vytvořena pomocí Unity herní platformy.

Aplikaci lze stáhnout **[ZDE](https://drive.google.com/file/d/1BBbptLxssqmlcz7bytOO-2H97Z4qp4ye/view?usp=sharing)**

**Samotná aplikace je podporována pro _Windows_.** Důvod je vysvětlen v realizaci níže.

## Popis realizace

Vzhled aplikace je vyzobrazen na obrázku (obr.1.), kde lze vidět všechny objekty uživatelského rozhraní (UI) a primitivního objektu *Plane*. UI představuje výběr či nastavování objektu Plane a samotné vykreslování čáry dle požadavků uživatele. Plane je mesh struktura vytvořena přímo Unity, která nabízí pár užitečných funkcí pro zjednodušení realizace.

<div align = "center">
<img width="650" height="350" src="https://user-images.githubusercontent.com/86803655/165863144-1f596855-632f-4417-9921-334ec9543978.PNG">

Obr.1.: Vzhled aplikace TrackSim
</div>

### Kolize a paprsky

Samotnou ideí bylo umožnit uživateli si naklikat body na rovinu, díky které by došlo k optimalizovanému řešení, neboli by uživatel naklikával vždy počáteční a koncové body úseček anebo oblouků. Toto by zejména u úseček snížilo výpis bodů na dva oproti bežného výpočtu bodů zavislé na vzdálenosti výpisu bodů o parametr *ratio*.

Byly použité *paprsky* (rays) pro naklikávání bodů na plane, kdy jsou při každém kliknutí na *Canvas* (rovina pro pokládání objektů uživatelského rozhraní - je zde připojena k obrazu hlavní kamery *main*) je přepočítána do *World* souřadného systému. Následně paprsky vrací kolize prvního (RayCast) anebo všech objektů (RayCastAll), kterými prochází, ale současně musí objekty tuto vlastnost kolize vlastnit. Proto k Planu byl připojen *komponent MeshCollidor* (komponenty jsou Assety objektu, které jsou k objektům připojeny a dodávájí jim příslušnou funkci), který vytvoří Collider mezi vrstvou mesh a meshBounds pro zachycení událostí (Events). Tento proces lze vidět na obrázku (obr.2.). Jak bylo předem řečeno, že Plane dává funkce, které ostatní primitivní objekty nenabízí a to metodu *ClosestPointOnPlane*, která vrací nejbližší bod v okolí průchodu paprsku Planem. Tímto je získáná přesnost pro pozdější výpočty, například pro výpočet oblouku.
  
<div align = "center">
<img width="700" height="450" src="https://user-images.githubusercontent.com/86803655/165862936-a08ef020-30ff-48a1-9351-4027204809bf.PNG">

Obr.2.: Použití paprsků a kolize pro získání bodů
</div>

### Výpočet oblouku (curve) a vedlejších čar (Triline)

Jak jde vidět na obrázku (obr.2.), úsečky nejsou problém zrealizovat, ale pro oblouky by uživatel musel naklikávat *n* počet bodů, proto byla použita Bezierova křivka. Je potřeba naklikat tři body - počáteční, střední a koncový bod, kde střední bod určuje zakřivení oblouku a není dále součástí exportovaných bodů. Díky této křivce jsou získány body vzdálené o parametr *r = 0,1* (lze ji změnit v programu) mezi počátečním a koncovým bodem. 

Vedlejší čáry jsou realizované za pomocí klonováním naklikaných bodů a posunutí je po ose X anebo ose Y dle směru vektoru mezi počatečním a koncovým bodem. Bohužel byl využit *Vector3.Angle()*, který vrací jenom kladné úhly (180-0-180), proto byl vytvářen virtuální souřadný systém, který zařazoval vektory do kvadrantů. Vektor byl  tedy vždy posunutý do počátku systému a směr udával kvadrant, který pak vyhodnotil stranu vedlejší čáry a její posunutí. Tento způsob zajistil správné posunutí bodů po jedné ose, ale i v situaci, kdy vektor měl +-45° sklon od vodorovné osy. V této situaci musí být body posunuty po obouch osách. 

Vedlejší oblouk byl vytvořen stejným způsobem, kdy všechny tři potřebné body byly posunuty a pak zavolaná funkce pro realizaci oblouku. Během přednášek bylo řečeno, že tloušťka mezery není determinována, ale přednášející tipoval 2cm. Toušťky všech čar jsou 2cm, proto pro čáry a oblouky je nastavena vzdálenost bodu *d = 4 [cm]*.

### Vizualizace čar a bodů

Hlavní trasa má vždy počátek v bodě [0,0,0] (dle pravidel soutěže) a následně jsou k nim přídány kolizní body. Počáteční a naklikané body jsou vizuálně vyznačeny primitivním objektem *krychle (Cube)*. Body jsou pospojeny pomocí _jedné_ čáry pomocí *LineRenderer* komponentu přidané pevně na *Empty Object*. LineRenderer zajišťuje vytváření komplexních čar pomocí zadáním počtu a pozice bodů. Vizualizace čar lze vidět na obázku (obr.3.)

<div align = "center">
<img width="700" height="400" src="https://user-images.githubusercontent.com/86803655/165862790-a6397b48-2023-4dcc-a82c-2055724bd7b4.PNG">

Obr.3.: Vizualizace čar a bodů
</div>

### Rastr

Pro zjednodušení naklikávání bodů uživatelem byl vytvořen *Rastr*, který lze vidět na obrázku (obr.1.). Bylo k tomu použit znovu objekt s LineRendererem. Následně byly vytvořeny čáry dle velikosti Planu. Bohužel Plane nenabízí *Transform* komponent (nabízí velikost, pozici apod.), tedy velikost musela být odměřena manuálně. Čáry byly následně naklonovány a posunuty o velikost *d*, kde určuje velikost mezery [cm] a je určen uživatelem, či případně rotovány.

### Export

Unity nenabízí *standalone File browser*, který by fungoval v Runtime aplikaci. Proto byl zde použita knihovna od **[SrejonKhan](https://github.com/SrejonKhan/AnotherFileBrowser)**. Bohužel tato knihovna nepodporuje Linux, ale jenom Windows a iOS, proto je tato aplikace omezena na Windows (pro iOS nebyla odzkoušena). táto knihovny ale nabízí funkce: výběr místa uložení souboru, filtrace a přepis výsledného formátu souboru.

*YAML soubor* je vytvořen za pomocí textového souboru, do kterého jsou body uloženy do vytvořené šablony, aby měl strukturu jako zdrojový soubor přiložený na počátku semestru. Body jsou ukládány postupně, kdy prvně jsou uloženy body hlavní trasy a následně body tvořící vedlejší čáry.

## TODO

Pro zjednodušení práce by bylo vhodné naimplementovat **odstranění posledního bodu či reset trasy**, protože při chybného nakliknutí anebo po rozmyslení je potřeba resetovat celou aplikaci. Nebyla tato funkce naimplementována, protože členové týmu to nepotřebovali. 

