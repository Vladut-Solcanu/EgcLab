Problema 0 :b) OpenGL  : 3.3.0 
Vendor  : Intel
Renderer: Intel(R) UHD Graphics 770

c) Da, versiunea este 3.3.0. Daca ar fi fost mai mica, creare ferestrei ar fi esuat.

Problema 1: c) Deoarece culoarea fundalului se schimba la fiecare cadru. Nu, animatia nu ar mai functiona in OnLoad.

Problema 2: a) În fragment shader, culoarea de bază a triunghiului se înmulțește cu uniforma uTint (prin instrucțiunea FragColor = vec4(vColor, 1.0) * uTint;). Prin urmare, valoarea (1.0f, 0.3f, 0.3f, 1.0f) menține roșul intact (înmulțit cu 1.0), dar atenuează verdele și albastrul la 30% (înmulțite cu 0.3), dând triunghiului o tentă roșiatică

c) Culoarea pixelilor din interior se obține prin interpolare în etapa de rasterizare. Sistemul calculează automat culoarea fiecărui fragment (pixel) ca o medie ponderată a culorilor celor trei vârfuri, în funcție de apropierea fragmentului față de fiecare vârf în parte.

d) Triunghiul devine colorat uniform, fără niciun degrade. Deoarece interpolarea din etapa de rasterizare calculează media ponderată a culorilor celor trei vârfuri, folosirea unor valori identice va produce exact aceeași nuanță pentru absolut toți pixelii din interior.

Problema 3. b) Folosim IsKeyPressed pentru ca actiunea sa se intample doar odata per apasare, daca am folosi IsKeyDown ar repeta constant actiunea cat tip tasta e apasata.

c) Daca se apeleaza o singura data, modul selectat va fi aplicat permanent. Aceasta ilustreaza principiul de masina cu stari al OpenGL.

Problema 4.  a) S-a schimbat valoarea uniformei uOffset care este trimisa la GPU.
d) Daca am folosi un pas fix pe cadru, viteza de deplasare a triunghiului ar deveni direct dependenta de numarul de cadre pe secunda. 
e) Părțile triunghiului care ies din limitele cubului NDC (Normalized Device Coordinates) sunt eliminate și nu mai sunt desenate pe ecran.

Problema 5. b) Imaginea afisata pe ecran nu se va schimba, iar modificarile nu vor fi vizibile, deoarece metodele precum SetPosition sau SetColor modifică doar tabloul Vertices care este stocat în memoria RAM a sistemului.

d) Problema 5 transferă semnificativ mai multe date către memoria GPU la fiecare cadru comparativ cu Problema 4. Pentru un model cu un milion de vârfuri, abordarea din Problema 5 ar însemna copierea iterativă a zeci de megabytes din memoria sistemului în memoria video la fiecare cadru randat.

e)Nu se va desena nimic pe ecran, rămânând vizibilă doar culoarea de fundal.

1.De ce sunt separate OnUpdateFrame și OnRenderFrame? Dați un exemplu de lucru care aparține fiecăreia.

Astfel separa logica aplicatiei de desenare. OnUpdateFrame: citirea tastelor sau calculul mișcării, iar OnRenderFrame: ștergerea ecranului și desenarea obiectelor.

2. De ce nu se apelează funcții GL.* în constructorul clasei Game, ci în OnLoad?

Deoarece fereastra și contextul OpenGL sunt create și devin utilizabile abia în momentul rulării metodei OnLoad, nu la inițializarea constructorului.

3. Ce conține un VAO și ce nu conține? Ce s-ar întâmpla la desenare dacă nu am lega niciun VAO?

VAO conține configurația datelor. Nu conține datele vârfurilor. Fără un VAO, OpenGL nu ar ști cum să citească și să interpreteze vârfurile din memorie, iar desenarea nu ar funcționa.

4. Care este diferența dintre un atribut (in din vertex shader) și o uniformă (uniform)? Dați câte un exemplu din proiect.

Un atribut are o valoare specifică pentru fiecare vârf în parte (ex: aPosition).O uniformă are aceeași valoare globală pentru toate vârfurile din cadrul unui apel de desenare(ex:uOffset). 

5. Ce înseamnă stride = 24 și offset = 12 în apelurile VertexAttribPointer din acest laborator?

Stride = 24 înseamnă distanța totală (24 de octeți) de la începutul unui vârf până la începutul următorului, iar offset = 12 înseamnă că datele pentru acel atribut specific încep după un decalaj de 12 octeți de la începutul vârfului.

6. De ce se deformează triunghiul când redimensionăm fereastra? Ce element al proiectului va rezolva această problemă?

Spațiul NDC (coordonatele normalizate) se întinde automat pe toată suprafața ferestrei, indiferent de forma acesteia. Problema va fi rezolvată de matricea de proiecție (clasa Camera).

7. Ce face SwapBuffers() și de ce are nevoie OpenGL de două buffere?

Funcția afișează imaginea transferând-o din bufferul ascuns în bufferul vizibil. Sunt necesare două buffere pentru ca desenarea să se facă invizibil (în spate), iar pe ecran să apară doar cadrele complet terminate.

8. Ce reprezintă args.Time și cum se folosește pentru ca mișcarea să nu depindă de numărul de cadre pe secundă?

Reprezintă timpul în secunde scurs între ultimul cadru și cel curent. Folosind formula viteză × args.Time, distanța parcursă se adaptează la timpul real, devenind independentă de variațiile de FPS.
