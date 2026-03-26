# Dragon Dungeon

## 1. Introducere

**Dragon Dungeon** este un joc video 2D de tip action-platformer, dezvoltat folosind motorul grafic Unity (C#). Proiectul a fost realizat ca temă pentru facultate, având ca scop implementarea mecanicilor clasice de platforming, a inteligenței artificiale de bază pentru inamici și a unui sistem modular de niveluri.

### Detalii Tehnice
* **Motor Grafic:** Unity 2022/2023 *(6.4)*
* **Limbaj de programare:** C#
* **Platformă:** PC (Windows)
* **Gen:** 2D Platformer / Acțiune

### Descrierea Jocului
În acest joc, jucătorul preia controlul unui personaj care trebuie să navigheze printr-o serie de încăperi periculoase. Pentru a supraviețui și a ajunge la ușa finală a fiecărui nivel, jucătorul trebuie să dea dovadă de reflexe rapide, folosind abilități precum *Wall Jump* (săritura pe pereți), combat (luptă) împotriva inamicilor și interacțiunea cu mediul înconjurător (săritura peste cutii,colectarea de vieți,checkpoint-uri).

## 2. Mecanici de Gameplay

Jocul este construit în jurul unor mecanici precise de mișcare și platforming, cerând jucătorului să analizeze mediul înainte de a acționa.

* **Sistemul de Mișcare:** Personajul se poate deplasa stânga-dreapta și poate efectua sărituri complexe cum ar fi sărituri multiple,hop,săritură lungă si săritura coyote(săritură in aer).
* **Wall Jump (Săritura pe perete):** O mecanică esențială pentru navigarea pe verticală. Jucătorul se poate prinde de pereți și poate sări de pe aceștia pentru a colecta obiecte aflate sus.
* **Interacțiunea cu Mediul:** Nivelurile conțin cutii statice. Deși nu pot fi împinse, ele sunt folosite strategic ca platforme improvizate sau ca ziduri pentru a executa *Wall Jumps* și a evita obstacole.
* **Luptă (Combat):** Personajul principal se apără exclusiv prin magie, atacând inamicii cu proiectile de foc (*Fireballs*). Această mecanică de *ranged combat* obligă jucătorul să mențină distanța față de inamicii care patrulează.
* **Sistemul de Camere (Rooms):** Nivelul nu este o hartă continuă, ci este împărțit în "camere". Când jucătorul trece printr-o ușă, camera de filmat (*Camera Controller*) se mută fluid spre următoarea secțiune a nivelului, oferind un sentiment de progresie clasică (stil Metroidvania/Zelda).



## 3. Inamici și Capcane (Obstacole)

Pentru a ajunge la ușa finală, jucătorul trebuie să supraviețuiască unui ecosistem variat de pericole:


### Inamici
Inamicii din joc au un comportament modular: ei pot fi plasați fie ca gărzi statice (păzind o platformă sau o ușă), fie în patrulă (mișcându-se stânga-dreapta pe o rută prestabilită). Aceștia se împart în două categorii:

* **Inamici Melee:** Sunt înarmați cu o sabie și atacă de aproape. Jucătorul trebuie să își calculeze bine poziția pentru a-i elimina cu *fireballs* înainte ca aceștia să ajungă în raza de lovire.
* **Inamici Ranged:** Atacă de la distanță trăgând cu proiectile. Indiferent dacă stau pe loc sau patrulează, aceștia declanșează dueluri de la distanță și obligă jucătorul să evite atacurile în timp ce ripostează.

### Capcane (Traps)
* **Spikes (Țepi statici):** Capcane clasice plasate pe podea sau pereți, care penalizează săriturile greșite.
* **Moving Saw (Fierăstrău mobil):** O capcană mecanică ce se deplasează constant pe o axă orizontală (stânga-dreapta).
* **Fire Trap (Capcană cu foc):** Erupe când jucătorul călcă pe ea și are un mic timp pentru a evada înainte de a se activa capcana, cerând sincronizare perfectă din partea lui.
* **Arrow Trap (Aruncător de săgeți):** Un mecanism pe pereți care trage cu proiectile pe o traiectorie liniară.
* **Spikehead (Inamic/Capcană de urmărire):** Un bloc masiv cu țepi care stă inactiv până când jucătorul intră în raza lui de acțiune, moment în care se lansează rapid spre direcția jucătorului și îl urmărește.

## 4. Interfața Grafică (UI) și Experiența Utilizatorului

Interfața jocului este concepută pentru a fi intuitivă și pentru a susține un flux de joc neîntrerupt, specific titlurilor retro:

* **Navigare exclusiv din tastatură (`SelectionArrow.cs`):** Pentru a menține imersiunea, meniurile nu necesită utilizarea mouse-ului. Un script personalizat controlează un `RectTransform` (o săgeată) care se mută fluid între opțiunile de UI pe baza input-ului (W/S sau Săgeți). Confirmarea se face cu tasta `Enter` sau `E`, apelând direct evenimentele de tip `Button.onClick.Invoke()`.
* **Managementul Stărilor (`UiManager.cs`):** Controlează logic și vizual ecranele de **Pause**, **Game Over** și **You Win**. La activarea acestora, managerul îngheață motorul fizic al jocului prin setarea `Time.timeScale = 0`.
* **Sistem de Viață Dinamic (`Healthbar.cs`):** Interfața afișează viața jucătorului printr-un element de tip `Image`, actualizând proprietatea `fillAmount` în timp real, direct proporțional cu viața rămasă a caracterului.
* **Setări Persistente (`VolumeText.cs` & `SoundManager.cs`):** Setările de volum (pentru muzică și efecte sonore) sunt salvate pe mașina utilizatorului folosind `PlayerPrefs`, astfel încât preferințele jucătorului rămân salvate chiar și după închiderea jocului.

---

## 5. Documentație Tehnică (Arhitectura Codului)

Proiectul este dezvoltat în C# (Unity) folosind principii solide de programare orientată pe obiecte (OOP), optimizare și management de stări. Codul este modularizat și împărțit în mai multe sisteme principale:

### 5.1. Concepte Avansate Utilizate
* **Moștenire (Inheritance):** Clasa `EnemyDamage.cs` acționează ca o clasă de bază (părinte). Clase precum `EnemyProjectile.cs` și `Spikehead.cs` moștenesc de la aceasta, reutilizând logica de bază pentru aplicarea daunelor (`TakeDamage`), respectând astfel principiul DRY (*Don't Repeat Yourself*).
* **Object Pooling:** În loc de a instanția (`Instantiate`) și distruge (`Destroy`) proiectilele de fiecare dată (ceea ce consumă resurse), scripturile `PlayerAttack.cs`, `RangedEnemy.cs` și `ArrowTrap.cs` folosesc o piscină de obiecte (array-uri predefinite de proiectile ascunse). Funcțiile caută un proiectil inactiv în ierarhie și îl reactivează, optimizând masiv performanța.
* **Singleton Pattern:** Folosit în `SoundManager.cs` (`public static SoundManager instance`) pentru a asigura existența unei singure instanțe a managerului de sunet la nivel global și supraviețuirea acesteia la trecerea prin niveluri (`DontDestroyOnLoad`).

### 5.2. Sistemul Jucătorului (Player Mechanics)
* **`PlayerMovement.cs`:** Gestionează input-ul, fizica (prin `Rigidbody2D`) și animațiile. Implementează mecanici complexe precum:
  * **Coyote Time:** O fereastră scurtă de timp în care jucătorul încă poate sări după ce a părăsit o platformă, prevenind frustrările legate de input.
  * **Wall Jump:** Folosește `Physics2D.BoxCast` pentru a detecta coliziunile laterale cu layer-ul "Wall", oprind gravitația și permițând săritura direcțională.
* **`Health.cs`:** Gestionează punctele de viață. Include un sistem de invulnerabilitate temporară (iFrames) implementat printr-un `IEnumerator` (Coroutine), care face sprite-ul jucătorului să clipească vizual și ignoră temporar coliziunile fizice cu layer-ul inamicilor.
* **`PlayerRespawn.cs`:** Salvează ultimul `Transform` atins ce poartă tag-ul "Checkpoint" și mută jucătorul, dar și camera principală, la acele coordonate în cazul în care viața ajunge la zero (dacă nu e Game Over complet).

### 5.3. Sistemul de Inamici (AI & Combat)
* **Inteligență Modulară:** Inamicii (Melee/Ranged) funcționează adesea în tandem cu `EnemyPatrol.cs`. Acest script gestionează mișcarea între două limite (LeftEdge/RightEdge) și implementează un temporizator (`idleTimer`) pentru momentele de întoarcere.
* **Detectarea Jucătorului (Vision):** Scripturile `MeeleEnemy.cs` și `RangedEnemy.cs` folosesc `Physics2D.BoxCast` pentru a arunca o "rază vizuală" în fața inamicului. Când jucătorul intră în această rază de detecție, scriptul de patrulare este dezactivat, iar inamicul atacă.

### 5.4. Mediu și Capcane (Environment & Traps)
* **`Spikehead.cs`:** Capcană inteligentă care calculează 4 direcții simultan (Sus, Jos, Stânga, Dreapta). Folosește `Physics2D.Raycast` constant (`checkTimer`). Când un raycast lovește layer-ul jucătorului, capcana oprește căutarea și se lansează violent pe acea axă.
* **`Firetrap.cs`:** Un *state machine* simplu bazat pe Coroutines. Evaluează stările `triggered` și `active`. Jucătorul are o fereastră de timp (activation delay vizualizat prin colorarea sprite-ului în roșu) să părăsească zona înainte de declanșarea efectivă a daunelor.
* **`Door.cs` & `Room.cs` & `CameraController.cs`:** Formează sistemul de progresie. Ușile detectează trecerea jucătorului, activează/dezactivează inamicii din camera respectivă (pentru a economisi resurse folosind `ActivateRoom()`) și instruiesc scriptul Camerei să facă o tranziție folosind funcția matematică `Vector3.SmoothDamp` pentru o mișcare fluidă.
