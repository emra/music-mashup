* * * Instruktioner för att köra API:et * * *

API:et är utvecklat i Visual Studio 2017

Solution-filen innehåller två st projekt:
- MusicAPI.Mashup
- MusicAPI.Mashup.Tests

För att köra API:et måste MusicAPI.Mashup vara satt som start up project (högerklicka på projekt -> set as startup project)
Starta sedan API:et (F5).

I API-dokumentation finns ett körbart exempelanrop

* * Kommentarer * *

- Nuvarande retry-logik om Music Brainz svarar med 503 bör kompletteras och/eller ersättas med förslagsvis en kö där anrop läggs in, en annan del av applikationen får sedan plocka från den kön och göra en mashup. Anrop som misslyckas kan då köas om ett begränsat antal gånger, sedan måste ett svar lämnas.
- Lasttest av mashup API:et behöver göras