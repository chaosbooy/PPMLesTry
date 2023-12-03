Projekt: PPMLesTry
Autor: Leon Przyklenk 3B Technik Programista Zespół Szkół Ekonomicznych im. gen. Stefana Roweckiego "Grota"
Używane: C# WPF
Opis: Aplikacja, która pozwala szyfrować ukryte wiadomości w obrazach bitmapowych (na razie .jpg,.gif,.png,.ppm);

Jak ma działać: Użytkownik Poda obraz i wiadomość (długość wiadomości jest zależna od wielkości obrazu) po czym po potwierdzeniu program bierze obraz zmienia go w typ .ppm szyfruje w nim wiadomość i oddaje spowrotem do użytkownika porównując starą z nową wersją (będzie przycisk pobierz który pobiera do folderu download). Obraz jest szyfrowany w regularnych odstępach wcześniej obliczonych jeżeli odstępy będą nieregularne (nie da się wyciągnąć stałej liczby odstępów) w "nagłówku" szyfru będzie zapisane ile razy jest dodany jeden odstęp do szyfru

Działanie teraźniejsze: można zrzucić obraz przy pomocy Drag&Drop lub go wybrać przy pomocy eksploratura pliku, podać wiadomość do zaszyfrowania, lecz po naciśnięciu zakoduj obraz się jedynie kopiuje do pliku typu ppm i pozostaje dopóki nie usunięty/zmieniony ręcznie lub zmieniony przez program; dodany jest także osobny plik stylów który zmienia domyślny wygląd Buttona oraz TextBoxa lecz to jeszcze będzie dopracowane aby zachowało wszystkie informacje na temat stylów tej aplikacji