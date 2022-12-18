<?php
//конфиг
$token = 'y0_AgAAAABl4J01AAiOLAAAAADTIhvZdGfIs24sRhqhWMv8Wa6WzOVu6TQ';
$publicURL =" ";
$folderId = uniqid();
//создаем папку

$folder = '/uploads/' . $folderId ;
$ch = curl_init('https://cloud-api.yandex.net/v1/disk/resources/?path=' . urlencode($folder));
curl_setopt($ch, CURLOPT_PUT, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, array('Authorization: OAuth ' . $token));
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_HEADER, false);
$res = curl_exec($ch);
curl_close($ch);
//открываем общий доступ к папке

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
$cnt = 0;
for($i=0;$i<4;++$i)
{
    if($_FILES['filename']['name'][$i]>0)
        ++$cnt;
}
if($cnt > 0) {
    for($i = 0; $i < $cnt; ++$i) {
        $target_file = "uploads/" . basename($_FILES["filename"]["name"][$i]);
        move_uploaded_file($_FILES["filename"]["tmp_name"][$i], $target_file);
//заливаем на диск
        $file = $target_file;
        $path = $folder . "/";
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
        unlink($target_file);
    }
}
//connect
$host='26.137.232.44';
$db = 'EdinoeOkno';
$username = 'Stas';
$password = '1';
$tableName="dev1.req_front";
$dbconn = pg_connect("host=$host port=5432 dbname=$db user=$username password=$password");

$name =" ";
$surname = " ";
$patronimic = " ";
$fac = " ";
$group = " ";
$email = " ";
if (isset ($_POST["name"]) && isset ($_POST["surname"]) &&
    isset ($_POST["patronimic"])  && isset ($_POST["fac"]) &&
    isset ($_POST["group"]) && isset ($_POST["email"])&& isset($_POST["tag"]))
{
    $tag = $_POST["tag"];
    $name =" ";
    $surname = " ";
    $patronimic = " ";
    $fac = " ";
    $group = " ";
    $email = " ";
    $name =$_POST["name"];
    $surname = $_POST["surname"];
    $patronimic = $_POST["patronimic"];
    $fac = $_POST["fac"];
    $group = $_POST["group"];
    $email = $_POST["email"];
}
//отправляем в бд
//(request_code, first_name, last_name, patronymic, email, faculty_code, student_group, doc_amount, dir_name, public_url)
$query = "insert into $tableName values ('$tag','$name','$surname','$patronimic','$email','$fac','$group','$cnt','$folderId','$publicURL');";
$result = pg_query($dbconn,$query );
pg_close($dbconn);
$new_url = '../Main.html';
echo $query;
header('Location: '.$new_url);
?>