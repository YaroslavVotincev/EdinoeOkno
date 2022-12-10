<?php
$host='26.137.232.44';
$db = 'EdinoeOkno';
$username = 'forms_guy';
$password = '1';
$tableName = "forms.response";

//$dbconn = pg_connect("host=$host port=5432 dbname=$db user=$username password=$password");

$text="";
$req = "";
$id=uniqid();
foreach ($_POST as $key => $value) {
    if(is_array($value))
    {
        if($value[0] == "radio-dop")
        {
            $req = $value[1] . "'" . $value[2]. "'";
            $q = "insert into $tableName values ($req);";
           // $r = pg_query($dbconn,$q);
            echo $q;
            echo '<br>';
            $req = "";
        }
        elseif($value[0]=="check-box-dop")
        {
            for($i= 1;$i<sizeof($value)-1;$i++)
            {
                $req = $req . $value[$i] . " " ."'". $value[sizeof($value)-1]."'";
            $q = "insert into $tableName values ($req);";
           // $r = pg_query($dbconn,$q);
            echo $q;
            echo '<br>';
            $req = "";
            }

        }
        elseif($value[0]=="check-box")
        {
            for($i= 1;$i<sizeof($value);$i++)
            {
                $req = $req . $value[$i];
                $q = "insert into $tableName values ($req);";
               // $r = pg_query($dbconn,$q);
                echo $q;
                echo '<br>';
                $req = "";
            }

        }
        else
        {
            //foreach ($value as $x)
                $req = $value[0] . "'". $value[1]. "'";
            $q = "insert into $tableName values ($req);";
            //$r = pg_query($dbconn,$q);
            echo $q;
            echo '<br>';
            $req = "";

        }
    }else
    {
        $req = $req . $value . " ";
        $q = "insert into $tableName values ($req);";
       // $r = pg_query($dbconn,$q);
        echo $q;
        echo '<br>';
        $req = "";
    }



}
//echo $req;
//$ins = "INSERT INTO tableName values ('idOtvexhayshego','idvoprosa','idOtveta','Text')"
print_r($_POST);
?>