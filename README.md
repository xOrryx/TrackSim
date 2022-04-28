# TrackSim

*Author - Petr Šopák*

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

### Kolize a rays

Samotným ideem bylo umožnit uživatele si naklikat body na rovinu, díky které by se došlo k optimalizovanému řešení, neboli by uživatel naklikával vždy počáteční a koncové body úseček anebo oblouku. Toto by zejména u úseček snížilo výpis bodů na dva oproti bežného výpočtu bodů zavislé na vzdálenosti výpisu o parametr *ratio*.

Byly použité *paprsky* (rays) pro naklikávání bodů na plane, kdy jsou při každém kliknutí na *Canvas* (rovina pro pokládání objektů uživatelského rozhraní - je zde připojena k obrazu hlavni kamery *main*) je přepočítaná do *World* souřadného systému. Následně paprsky vrací kolize prvního (RayCast) anebo všech objektů (RayCastAll), kterými prochází, ale současně musí objekty tuto vlastnost kolize vlastnit. Proto k Planu byl připojen *komponent MeshCollidor* (komponenty jsou Assety objektu, které jsou k objektům připojeny a dodávájí jim přislušnou funkci), který vytvoří Collider mezi vrstvou mesh a meshBounds pro zachytávání událostí (Events). Tento proces Lze vidět na obrázku (obr.2.). Jak bylo předem řečeno, že Plane dává funkce, které ostatní primitivní objekty nedodávají a to methodu *ClosestPointOnPlane*, který vrací nejbližší bod v okoli projití paprsku Planem. Tímto je získáná přesnost během následného výpočtu mezi body například pro výpočet oblouku.

<div align = "center">
<img width="550" height="400" src="/uploads/1695c35480e635f1b0c9bfe174f6357b/char1.PNG">

Obr.2.: Použití ray a kolize pro získávaní bodů
</div>

