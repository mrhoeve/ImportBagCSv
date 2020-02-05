select a.Straat, n.Huisnummer, a.Postcode, p.Naam, g.Naam, pro.Naam from Nummers n join Adressen a on n.AdresId=a.Id join Plaatsen p on a.PlaatsId=p.Id join Gemeenten g on p.GemeenteId=g.Id 
join Provincies pro on g.ProvincieId=pro.Id where a.Postcode='7948AC' and n.Huisnummer=20;

select Naam from Plaatsen where Naam in (select Naam from Plaatsen group by Naam having count(*) > 1);