<?php
$token = 'y0_AgAAAABl4J01AAiOLAAAAADTIhvZdGfIs24sRhqhWMv8Wa6WzOVu6TQ';
//заливаесм фаил на сервер
$target_dir = "uploads/";
$publicURL=" ";


   $cnt = count($_FILES['filename']['name']);

   if($_FILES['filename']['name'][1]<0)
       $cnt = $cnt -1;
    echo $cnt;
    if($cnt > 0) {
    for($i = 0; $i < $cnt; ++$i) {
        // Тут выполняем всякое разное с файлом. Путь к нему в $_FILES['mail_file']['tmp_name'][$i]
        $target_file = $target_dir . basename($_FILES["filename"]["name"][$i]);
        move_uploaded_file($_FILES["filename"]["tmp_name"][$i], $target_file);
//заливаем на диск
        $file = $target_file;
        $path = '/uploads/';

// Запрашиваем URL для загрузки.
        $ch = curl_init('https://cloud-api.yandex.net/v1/disk/resources/upload?path=' . urlencode($path . basename($file)));
        curl_setopt($ch, CURLOPT_HTTPHEADER, array('Authorization: OAuth ' . $token));
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
        curl_setopt($ch, CURLOPT_HEADER, false);
        $res = curl_exec($ch);
        curl_close($ch);
        $res = json_decode($res, true);
        if (empty($res['error'])) {
            // Если ошибки нет, то отправляем файл на полученный URL.
            $fp = fopen($file, 'r');
            $ch = curl_init($res['href']);
            curl_setopt($ch, CURLOPT_PUT, true);
            curl_setopt($ch, CURLOPT_UPLOAD, true);
            curl_setopt($ch, CURLOPT_INFILESIZE, filesize($file));
            curl_setopt($ch, CURLOPT_INFILE, $fp);
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
            curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
            curl_setopt($ch, CURLOPT_HEADER, false);
            curl_exec($ch);
            $http_code = curl_getinfo($ch, CURLINFO_HTTP_CODE);
            curl_close($ch);
        }
//открываем общий доступ к файлу

        $ch = curl_init('https://cloud-api.yandex.net/v1/disk/resources/publish?path='. urlencode($path . basename($file)));
        curl_setopt($ch, CURLOPT_HTTPHEADER, array('Authorization: OAuth ' . $token));
        curl_setopt($ch, CURLOPT_PUT, true);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
        curl_setopt($ch, CURLOPT_HEADER, false);
        $html = curl_exec($ch);
        curl_close($ch);
        $html = json_decode($html, true);
//получаем public URL

        $ch = curl_init('https://cloud-api.yandex.net/v1/disk/resources?path=' . urlencode($path . basename($file)));
        curl_setopt($ch, CURLOPT_HTTPHEADER, array('Authorization: OAuth ' . $token));
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        curl_setopt($ch, CURLOPT_HEADER, false);

        $res = curl_exec($ch);
        $res = json_decode($res, true);





        $publicURL =$publicURL ." ". $res['public_url'];
    }

}
echo $publicURL;



?>