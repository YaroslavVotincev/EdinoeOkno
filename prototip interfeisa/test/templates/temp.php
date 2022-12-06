<?php
$text="";
$req = "";
$id=uniqid();
foreach ($_POST as $key => $value) {
    if(is_array($value))
    {
        if($value[0] == "radio-dop")
        {
            for($i= 1;$i<sizeof($value);$i++)
                $req = $req . $value[$i] . " ";

            echo $req;
            echo '<br>';
            $req = "";
        }
        elseif($value[0]=="check-box-dop")
        {
            for($i= 1;$i<sizeof($value)-1;$i++)
            {
                $req = $req . $value[$i] . " " . $value[sizeof($value)-1];
            echo $req;
            echo '<br>';
            $req = "";}

        }
        elseif($value[0]=="check-box")
        {
            for($i= 1;$i<sizeof($value);$i++)
            {
                $req = $req . $value[$i];
                echo $req;
                echo '<br>';
                $req = "";}

        }
        else
        {
            foreach ($value as $x)
                $req = $req . $x . " ";
            echo $req;
            echo '<br>';
            $req = "";

        }
    }else
    {
        $req = $req . $value . " ";
        echo $req;
        echo '<br>';
        $req = "";
    }



}
//echo $req;
//$ins = "INSERT INTO tableName values ('idOtvexhayshego','idvoprosa','idOtveta','Text')"
print_r($_POST);
?>