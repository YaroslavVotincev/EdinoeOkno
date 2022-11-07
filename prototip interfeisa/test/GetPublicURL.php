<?php
$token = 'y0_AgAAAABl4J01AAiOLAAAAADTIhvZdGfIs24sRhqhWMv8Wa6WzOVu6TQ';
$path='/uploads/1.jpg';

$ch = curl_init('https://cloud-api.yandex.net/v1/disk/resources?path=' . urlencode($path));
curl_setopt($ch, CURLOPT_HTTPHEADER, array('Authorization: OAuth ' . $token));
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HEADER, false);

$res = curl_exec($ch);
$res = json_decode($res, true);
print_r($res['public_url']);
?>