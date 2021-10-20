# ToDoList
Kursa projekts - ToDoList


Artis iesaka PostgreSQL datubaze

Datu bāzes pārvaldības sistēma : Mysql
entity core Framework

//======================= @TODO  =======================
Pamat funkcijas:
1) Pievienot ierakstu
2) Dzest ierakstu ( ar comentari )
3) Regiget ierakstu
4) Definet atbildigo 
4) Definet piority


array table_status (new,processed, compleated ) ??? (iespejams use defined array)
array table_Priority (asap,hight,normal,Low)

Datubaze: 

1) tabula ar ierakstiem (id, atbildigais_id, ToDo_text, Priority, Status_id, pievienosanas_datums,is_deleted )
2) table_deleted (ieraksta_id, delete reason,date_time,atbildigas_id)
3) table_users(id,atbildigais_FullName, e_mail, phone)
4) _log (id,ieraksta_id,atbildigais_id,changes_text,datetime )



