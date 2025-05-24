# Transportation

# Használt technológia:
-ASP.NET Core Web API
-In-memory Data storage
-c# languge

#Felépítés
rétegzett alkalmazás ami tartalmaz
-Data
-Endpoint
-Entities
-Logic

#Data:
-Csak az Entities réteget ismeri
-TransportaionContext.cs: Az adatbázis kezelésért felel
-Repository.cs: CRUD műveleteket biztosít bármely Entityre

#Endpoint
-Minden réteget ismer
-VehicleController.cs: a HTTP kéréseket kezeli és továbbítja őket a Logic réteg felé
  #végpontok:
    -Get /vehicle ->	Összes jármű lekérdezése.
    -GET	/vehicle/{id} -> Egy jármű lekérdezése ID alapján.
    -POST	/vehicle -> Új jármű létrehozása DTO alapján.
    -PUT	/vehicle/{id} -> Létező jármű frissítése.
    -DELETE	/vehicle/{id} -> Jármű törlése ID alapján.
    -POST	/vehicle/trip-suggestions -> Javasolt járművek listázása Utas- és távolságigény alapján.
Program.cs: 
  -Az egész rendszer beállítása.
  -Szolgáltatások regisztrálása.
  -Alkalmazás elindítása.
#Entities
-DTO-kat és az Vehicle entityt tartalamazza

#Logic
-A Data és az Entities réteget ismeri
-DtoProvider.cs: Mapper segítségével átalakítja a Vehicle-t VehicleViewDto-ra és a VehicleCreateUpdateDto-t Vehicle-re.
-VehicleLogic.cs:
  -Az alkalmazás üzleti logikája
  -A VehicleControllertől kapott kérések logikáját végzi el és a Repository segítségével hajtja végre.
  -CRUD műveletek DTO segítségével
  -GetTripSuggestion(int passengers, int distance):
    -Kiválasztja azokat a járműveket, amelyek hatótávja elég a távolság megtételéhez.
    -Kombinációkat generál különböző számú járművel, amelyek képesek elvinni az összes utast.
    -Minden kombinációra kiszámítja a profitot.
    -A legjobb 1000 javaslatot visszaadja (Túl nagy adat esetén ne akadjon ki a swagger).
    -Használja:
      -GenerateCombinationsBasedOnK(List<Vehicle> vehicles, int k): kombinációkat generál adott méretre. Méret: O(k*(n alatt k))
      -CalculateProfit(List<Vehicle> vehicles, int distance, int passengers): a kombinációk nyereségét számítja ki.
