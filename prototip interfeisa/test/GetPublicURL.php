<?php
$token = 'y0_AgAAAABl4J01AAiOLAAAAADTIhvZdGfIs24sRhqhWMv8Wa6WzOVu6TQ';
//заливаесм фаил на сервер
//говно не работает

$publicURL=" ";
$cnt = 1;

//создаем папку

$folder = '/uploads/' . uniqid();
$ch = curl_init('https://cloud-api.yandex.net/v1/disk/resources/?path=' . urlencode($folder));
curl_setopt($ch, CURLOPT_PUT, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, array('Authorization: OAuth ' . $token));
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_HEADER, false);
$res = curl_exec($ch);
curl_close($ch);
//открываем общий доступ к файлу

$ch = curl_init('https://cloud-api.yandex.net/v1/disk/resources/publish?path='. urlencode($folder));
curl_setopt($ch, CURLOPT_HTTPHEADER, array('Authorization: OAuth ' . $token));
curl_setopt($ch, CURLOPT_PUT, true);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_HEADER, false);
$html = curl_exec($ch);
curl_close($ch);
$html = json_decode($html, true);
//получаем public URL

$ch = curl_init('https://cloud-api.yandex.net/v1/disk/resources?path=' . urlencode($folder));
curl_setopt($ch, CURLOPT_HTTPHEADER, array('Authorization: OAuth ' . $token));
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HEADER, false);

$res = curl_exec($ch);
$res = json_decode($res, true);

$publicURL = $res['public_url'];
echo $publicURL;
?>