INCLUDE ../globals.ink

-> punong_guro

=== punong_guro ===
MAGANDANG UMAGA! Ikaw ba ay nagtataka?
    * [OO] -> oo_response
    * [HINDI] -> hindi_response

=== oo_response ===
Mahusay! Ang lugar na ito ay tinatawag na Labirinto. 
Ito ay hindi lamang basta kasaysayan—ito ay isang pagsasalamin ng iyong sariling paglalakbay. 
Haharapin mo ang mga pagsubok at matutuklasan mo ang mga lihim ng nakaraan. 
Handa ka bang tanggapin ang hamon?
    * [Tanggapin] -> accept_challenge
    * [Tanggihan] -> decline_challenge

=== accept_challenge ===
Mahusay! Ngayon ay magsisimula na ang iyong pagsubok. 
# START_CHALLENGE_QUIZ
Congrats.
-> END

=== decline_challenge ===
Naiintindihan ko. Hindi lahat ay handa sa mga hamon ng Labirinto. Babalik ka na lamang kung handa ka na. 
# START_CHALLENGE_DRAGDROP
-> END

=== hindi_response ===
Hindi ka nag-iisa sa iyong pagdududa. 
Ang lugar na ito ay puno ng mga aral, at makikita mo ang kahalagahan ng kasaysayan sa iyong sariling buhay. 
Handa ka na bang tuklasin ang mga lihim nito?
-> END
