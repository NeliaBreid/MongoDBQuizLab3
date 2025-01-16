# Labb 3 - Quiz Configurator

Jag har byggt en applikation för att konfigurera quiz-frågor och köra quiz-rundor. Appen är skriven i WPF och XAML och byggas på Model-View-ViewModel (MVVM)-arkitektur. Appen består i huvudsak av två delar ,en “configurator” för att skapa paket med frågor; och en “player” där man kan köra quiz-rundor.


## Configuration Mode

Man kan bygga “Question Packs”, det vill säga “paket” med frågor. Man kan lägga till, ta bort, och redigera befintliga frågor. Alla frågor har fyra alternativ, varav ett korrekt. Man kan även ändra inställningar för själva paketet “Pack Options”: Välja namn, märka upp med svårighetsgrad, samt sätta tidsgräns på frågorna. Man kan även skapa nya “Packs”, och ta bort befintliga.

Det går att lägga till / ta bort frågor på flera sätt: Via menyn, Via snabbknappar på tangentbordet, och via knappar i appen. Även “Pack Options” går att öppna på alla 3 sätt.


## Play Mode

När man startar play mode visar hur många frågor det är i det aktiva “Frågepaketet”, samt vilken fråga man är på i turordning. Man kan alltså svara på alla frågor i paketet innan man får ett resultat som visar hur många rätt man hade. Ordningen frågorna visas i ska dock slumpas från gång till gång; likadant med ordningen som svarsalternativen visas i. Det finns en timer som räknar ner (betänketid per fråga ska gå att ställa in när man konfigurerar packen), och efter man klickat på ett svar (eller tiden tar slut) får användaren feedback på om man svarat rätt/fel, samt vilket det korrekta svarsalternativet är.


## Menyn

Menyn har ikoner för de olika alternativen (t.ex med font-awesome). Menyalternativen går att aktivera från tangentbordet både med snabbvalsknappar, och med alt-knappen, till exempel Ctrl+O, samt Alt+E, O för “Pack Options”.

## **Full Screen**

Det ska finnas ett menyalternativ för att köra programmet i helskärmsläge.
