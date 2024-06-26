ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
create role TRGKHOA;
--drop role TRGKHOA;
ALTER SESSION SET "_ORACLE_SCRIPT" = FALSE;

--NHU MOT NGUOI DUNG CO VAI TRO GIANG VIEN
grant GIANGVIEN to TRGKHOA;
grant NVCOBAN to TRGKHOA;
grant connect to TRGKHOA;

--TAO VIEW TREN QUAN HE PHAN CONG DOI VOI CAC HOC PHAN QUAN LY BOI DON VI VAN PHONG KHOA
create or replace view ADMIN.UV_TRGKHOA_PHANCONG
as
    select PC.*
    from ADMIN.PROJECT_PHANCONG PC, ADMIN.PROJECT_HOCPHAN HP, ADMIN.PROJECT_DONVI DV
    where PC.MAHP = HP.MAHP
        AND HP.MADV = DV.MADV
        AND dv.tendv = 'VP KHOA';
create or replace view ADMIN.UV_TRGKHOA_HOCPHAN
as
    select HP.*
    from ADMIN.PROJECT_HOCPHAN HP, ADMIN.PROJECT_DONVI DV
    where HP.MADV = DV.MADV
        AND dv.tendv = 'VP KHOA';

grant SELECT on ADMIN.UV_TRGKHOA_HOCPHAN to TRGKHOA;
grant INSERT on ADMIN.PROJECT_KHMO to TRGKHOA;
        
--THEM, XOA, CAP, NHAT ADMIN.UV_TRGKHOA_PHANCONG
grant SELECT, INSERT, DELETE, UPDATE on ADMIN.UV_TRGKHOA_PHANCONG to TRGKHOA;

--XEM, THEM, XOA, SUA, TREN BANG NHAN SU
grant SELECT, INSERT, DELETE, UPDATE on ADMIN.PROJECT_NHANSU to TRGKHOA;

--XEM TAT CA CAC BANG
grant select any table to TRGKHOA;
/
declare
    cursor cur_TRGDONVI is (select * from ADMIN.PROJECT_NHANSU where VAITRO = 'TRGKHOA');
    STRSQL varchar2(1000);
    result boolean;
begin
    for row_TRGDONVI in cur_TRGDONVI
    loop
        result := admin.isUserExists(row_TRGDONVI.MANV);
        if(result = false) then
            STRSQL := 'CREATE USER ' || row_TRGDONVI.MANV || ' identified by 123';
            EXECUTE IMMEDIATE (STRSQL);
        end if;
        STRSQL := 'GRANT TRGKHOA to ' || row_TRGDONVI.MANV;
        EXECUTE IMMEDIATE (STRSQL);
        STRSQL := 'GRANT CONNECT to ' || row_TRGDONVI.MANV;
        EXECUTE IMMEDIATE (STRSQL);
    end loop;
end;
/
declare
    cursor cur_TRGDONVI is (select * from ADMIN.PROJECT_NHANSU where VAITRO = 'TRGKHOA');
    STRSQL varchar2(1000);
begin
    for row_TRGDONVI in cur_TRGDONVI
    loop
        STRSQL := 'DROP USER ' || row_TRGDONVI.MANV;
        EXECUTE IMMEDIATE (STRSQL);
    end loop;
end;
/
