<?php echo
$token = 'y0_AgAAAABl4J01AAiOLAAAAADTIhvZdGfIs24sRhqhWMv8Wa6WzOVu6TQ';
// Файл на Диске.
$yd_file = '/uploads/1231234.docx';

// Директория, куда будет сохранен файл.
$path = 'C:\Users\artem\Desktop\asdasd\EdinoeOkno\prototip interfeisa\HTML\uploads';

$ch = curl_init('https://cloud-api.yandex.net/v1/disk/resources/download?path=' . urlencode($yd_file));
curl_setopt($ch, CURLOPT_HTTPHEADER, array('Authorization: OAuth ' . $token));
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HEADER, false);

$res = curl_exec($ch);
curl_close($ch);

$res = json_decode($res, true);
if (empty($res['error']))
{
    $file_name = $path . '/' . basename($yd_file);
    $ch = curl_init($res['href']);
    curl_setopt($ch, CURLOPT_FOLLOWLOCATION, 1);
    curl_setopt($ch, CURLOPT_HTTPHEADER, array('Authorization: OAuth ' . $token));
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
    curl_setopt($ch, CURLOPT_HEADER, false);
    $res=curl_exec($ch);
    curl_close($ch);
    file_put_contents($file_name, $res);
}
print_r($res['public_url']);
?>