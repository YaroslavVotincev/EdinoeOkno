<?php
$host='26.137.232.44';
$db = 'EdinoeOkno';
$username = 'Artem';
$password = '1';

$dbconn = pg_connect("host=$host port=5432 dbname=$db user=$username password=$password");

if (!$dbconn) {
    die('Could not connect');
}
else {
    echo ("Connected to local DB");
}
?>