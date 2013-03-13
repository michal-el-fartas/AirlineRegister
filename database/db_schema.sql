--
-- PostgreSQL database dump
--

-- Dumped from database version 9.2.1
-- Dumped by pg_dump version 9.2.0
-- Started on 2013-03-13 21:16:42

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 180 (class 3079 OID 11727)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 1990 (class 0 OID 0)
-- Dependencies: 180
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

--
-- TOC entry 193 (class 1255 OID 16524)
-- Name: dystans(integer, integer); Type: FUNCTION; Schema: public; Owner: micmax93
--

CREATE FUNCTION dystans(l_wylotu integer, l_przylotu integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$DECLARE
	AX INTEGER;
	AY INTEGER;
	ZX INTEGER;
	ZY INTEGER;
	DIST_X INTEGER;
	DIST_Y INTEGER;
	RESULT REAL;
BEGIN

SELECT WSP_X, WSP_Y
INTO AX, AY
FROM LOTNISKA
WHERE ID_LOTNISKA=L_WYLOTU;

SELECT WSP_X, WSP_Y
INTO ZX, ZY
FROM LOTNISKA
WHERE ID_LOTNISKA=L_PRZYLOTU;

DIST_X:=AX-ZX;
DIST_Y:=AY-ZY;

DIST_X:=DIST_X*DIST_X;
DIST_Y:=DIST_Y*DIST_Y;

RESULT:=|/(DIST_X+DIST_Y);
RETURN CAST(RESULT AS INT);

END$$;


ALTER FUNCTION public.dystans(l_wylotu integer, l_przylotu integer) OWNER TO micmax93;

--
-- TOC entry 194 (class 1255 OID 16531)
-- Name: usun_stare(); Type: FUNCTION; Schema: public; Owner: micmax93
--

CREATE FUNCTION usun_stare() RETURNS void
    LANGUAGE sql
    AS $$DELETE FROM LOTY WHERE DATA_WYLOTU<NOW();$$;


ALTER FUNCTION public.usun_stare() OWNER TO micmax93;

--
-- TOC entry 177 (class 1259 OID 16468)
-- Name: klienci_seq; Type: SEQUENCE; Schema: public; Owner: micmax93
--

CREATE SEQUENCE klienci_seq
    START WITH 10000
    INCREMENT BY 1
    MINVALUE 10000
    MAXVALUE 99999
    CACHE 1;


ALTER TABLE public.klienci_seq OWNER TO micmax93;

--
-- TOC entry 1991 (class 0 OID 0)
-- Dependencies: 177
-- Name: klienci_seq; Type: SEQUENCE SET; Schema: public; Owner: micmax93
--

SELECT pg_catalog.setval('klienci_seq', 10016, true);


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 171 (class 1259 OID 16430)
-- Name: klienci; Type: TABLE; Schema: public; Owner: micmax93; Tablespace: 
--

CREATE TABLE klienci (
    id_klienta integer DEFAULT nextval('klienci_seq'::regclass) NOT NULL,
    imie text,
    nazwisko text,
    data_urodzenia timestamp without time zone,
    adres text,
    telefon text,
    bonusowe_mile integer
);


ALTER TABLE public.klienci OWNER TO micmax93;

--
-- TOC entry 175 (class 1259 OID 16461)
-- Name: lotniska_seq; Type: SEQUENCE; Schema: public; Owner: micmax93
--

CREATE SEQUENCE lotniska_seq
    START WITH 10000
    INCREMENT BY 1
    MINVALUE 10000
    MAXVALUE 99999
    CACHE 1;


ALTER TABLE public.lotniska_seq OWNER TO micmax93;

--
-- TOC entry 1992 (class 0 OID 0)
-- Dependencies: 175
-- Name: lotniska_seq; Type: SEQUENCE SET; Schema: public; Owner: micmax93
--

SELECT pg_catalog.setval('lotniska_seq', 10008, true);


--
-- TOC entry 169 (class 1259 OID 16400)
-- Name: lotniska; Type: TABLE; Schema: public; Owner: micmax93; Tablespace: 
--

CREATE TABLE lotniska (
    id_lotniska integer DEFAULT nextval('lotniska_seq'::regclass) NOT NULL,
    kraj text,
    miejscowosc text,
    nazwa text,
    typ character varying(20),
    maks_rozmiar character varying(20),
    wsp_x integer,
    wsp_y integer
);


ALTER TABLE public.lotniska OWNER TO micmax93;

--
-- TOC entry 173 (class 1259 OID 16457)
-- Name: loty_seq; Type: SEQUENCE; Schema: public; Owner: micmax93
--

CREATE SEQUENCE loty_seq
    START WITH 10000
    INCREMENT BY 1
    MINVALUE 10000
    MAXVALUE 99999
    CACHE 1;


ALTER TABLE public.loty_seq OWNER TO micmax93;

--
-- TOC entry 1993 (class 0 OID 0)
-- Dependencies: 173
-- Name: loty_seq; Type: SEQUENCE SET; Schema: public; Owner: micmax93
--

SELECT pg_catalog.setval('loty_seq', 10012, true);


--
-- TOC entry 170 (class 1259 OID 16410)
-- Name: loty; Type: TABLE; Schema: public; Owner: micmax93; Tablespace: 
--

CREATE TABLE loty (
    id_lotu integer DEFAULT nextval('loty_seq'::regclass) NOT NULL,
    data_wylotu timestamp without time zone,
    czas_lotu integer,
    lotnisko_docelowe integer,
    lotnisko_zrodlowe integer,
    samolot integer
);


ALTER TABLE public.loty OWNER TO micmax93;

--
-- TOC entry 174 (class 1259 OID 16459)
-- Name: samoloty_seq; Type: SEQUENCE; Schema: public; Owner: micmax93
--

CREATE SEQUENCE samoloty_seq
    START WITH 10000
    INCREMENT BY 1
    MINVALUE 10000
    MAXVALUE 99999
    CACHE 1;


ALTER TABLE public.samoloty_seq OWNER TO micmax93;

--
-- TOC entry 1994 (class 0 OID 0)
-- Dependencies: 174
-- Name: samoloty_seq; Type: SEQUENCE SET; Schema: public; Owner: micmax93
--

SELECT pg_catalog.setval('samoloty_seq', 10023, true);


--
-- TOC entry 168 (class 1259 OID 16395)
-- Name: samoloty; Type: TABLE; Schema: public; Owner: micmax93; Tablespace: 
--

CREATE TABLE samoloty (
    id_samolotu integer DEFAULT nextval('samoloty_seq'::regclass) NOT NULL,
    marka text,
    model text,
    zasieg integer,
    miejsca integer,
    biznes_klasa integer,
    predkosc integer,
    typ text
);


ALTER TABLE public.samoloty OWNER TO micmax93;

--
-- TOC entry 178 (class 1259 OID 16519)
-- Name: loty_view; Type: VIEW; Schema: public; Owner: micmax93
--

CREATE VIEW loty_view AS
    SELECT l.id_lotu, l.data_wylotu, l.czas_lotu, ((s.miejscowosc || ' '::text) || s.nazwa) AS wylot, ((d.miejscowosc || ' '::text) || d.nazwa) AS przylot, ((p.marka || ' '::text) || p.model) AS samolot, s.id_lotniska AS idwylot, d.id_lotniska AS idprzylot, p.id_samolotu AS idsamolot FROM loty l, lotniska s, lotniska d, samoloty p WHERE (((l.lotnisko_docelowe = d.id_lotniska) AND (l.lotnisko_zrodlowe = s.id_lotniska)) AND (l.samolot = p.id_samolotu));


ALTER TABLE public.loty_view OWNER TO micmax93;

--
-- TOC entry 176 (class 1259 OID 16463)
-- Name: rezerwacje_seq; Type: SEQUENCE; Schema: public; Owner: micmax93
--

CREATE SEQUENCE rezerwacje_seq
    START WITH 10000
    INCREMENT BY 1
    MINVALUE 10000
    MAXVALUE 99999
    CACHE 1;


ALTER TABLE public.rezerwacje_seq OWNER TO micmax93;

--
-- TOC entry 1995 (class 0 OID 0)
-- Dependencies: 176
-- Name: rezerwacje_seq; Type: SEQUENCE SET; Schema: public; Owner: micmax93
--

SELECT pg_catalog.setval('rezerwacje_seq', 10010, true);


--
-- TOC entry 172 (class 1259 OID 16435)
-- Name: rezerwacje; Type: TABLE; Schema: public; Owner: micmax93; Tablespace: 
--

CREATE TABLE rezerwacje (
    id_rezerwacji integer DEFAULT nextval('rezerwacje_seq'::regclass) NOT NULL,
    id_klienta integer,
    id_lotu integer,
    klasa character varying(15)
);


ALTER TABLE public.rezerwacje OWNER TO micmax93;

--
-- TOC entry 179 (class 1259 OID 16540)
-- Name: rezerwacje_view; Type: VIEW; Schema: public; Owner: micmax93
--

CREATE VIEW rezerwacje_view AS
    SELECT r.id_rezerwacji AS rezerwacja, k.id_klienta AS idklient, ((k.nazwisko || ' '::text) || k.imie) AS klient, r.id_lotu AS lot, r.klasa FROM rezerwacje r, klienci k WHERE (r.id_klienta = k.id_klienta);


ALTER TABLE public.rezerwacje_view OWNER TO micmax93;

--
-- TOC entry 1981 (class 0 OID 16430)
-- Dependencies: 171
-- Data for Name: klienci; Type: TABLE DATA; Schema: public; Owner: micmax93
--

COPY klienci (id_klienta, imie, nazwisko, data_urodzenia, adres, telefon, bonusowe_mile) FROM stdin;
10000	Jan	Kowalski	1993-10-23 00:00:00	Poznań, Piotrowo 6	512993484	10
10002	Jurek	Drugi	2000-05-05 00:00:00	Radom	0324215111	3
10001	Adam	Nowak	1980-01-05 00:00:00	Warszawa	36315542	14
10004	Kamil	Obecny	1992-10-08 00:00:00	Warszawka	0801	40
10003	Aga	Nowak	1990-02-01 00:00:00	Luboń	0700	43
10007	Roman	Polański	1970-01-01 00:00:00	Katowice	646821	200
10012	Krzysztof	Jarzyna	2013-01-18 00:00:00	Szczecin	99999999	200
10008	Zbigniew	Ciekawski	2013-02-13 09:00:00	Białystok	2351963	15
10014	Euzebiusz	Dąbrowiecki	2013-01-16 00:00:00	Oborniki	54373226	19
10009	Mirosław	Kuroń	1993-10-23 00:00:00	Sopot	62747271	16
10016	Test	Test	2009-01-14 00:00:00	Poznań	45262561	0
\.


--
-- TOC entry 1979 (class 0 OID 16400)
-- Dependencies: 169
-- Data for Name: lotniska; Type: TABLE DATA; Schema: public; Owner: micmax93
--

COPY lotniska (id_lotniska, kraj, miejscowosc, nazwa, typ, maks_rozmiar, wsp_x, wsp_y) FROM stdin;
10000	POLSKA	POZNAN	ŁAWICA	ŚREDNIE	MIEDZYNARODOWE	5	2000
10001	POLSKA	WARSZAWA	OKĘCIE	DUŻE	KRAJOWE	1000	10
10002	NIEMCY	FRANKFURT	FRA	DUŻE	MIEDZYNARODOWE	500	200
10003	TURCJA	ISTANBUŁ	Akrahm	DUŻE	MIEDZYNARODOWE	200	700
10005	USA	Nowy Jork	JFK	ŚREDNIE	MIEDZYNARODOWE	320	3000
10004	POLSKA	POZNAŃ	KRZESINY	MAŁE	MIEDZYNARODOWE	2400	80
10007	MEXICO	La Ciudad de Mexico	Mexico	MAŁE	MIEDZYNARODOWE	0	0
\.


--
-- TOC entry 1980 (class 0 OID 16410)
-- Dependencies: 170
-- Data for Name: loty; Type: TABLE DATA; Schema: public; Owner: micmax93
--

COPY loty (id_lotu, data_wylotu, czas_lotu, lotnisko_docelowe, lotnisko_zrodlowe, samolot) FROM stdin;
10012	2013-03-16 00:00:00	135	10004	10002	10013
\.


--
-- TOC entry 1982 (class 0 OID 16435)
-- Dependencies: 172
-- Data for Name: rezerwacje; Type: TABLE DATA; Schema: public; Owner: micmax93
--

COPY rezerwacje (id_rezerwacji, id_klienta, id_lotu, klasa) FROM stdin;
\.


--
-- TOC entry 1978 (class 0 OID 16395)
-- Dependencies: 168
-- Data for Name: samoloty; Type: TABLE DATA; Schema: public; Owner: micmax93
--

COPY samoloty (id_samolotu, marka, model, zasieg, miejsca, biznes_klasa, predkosc, typ) FROM stdin;
10001	Boeing	767-400ER	10415	304	30	980	DUŻY
10002	Boeing	777-300ER	14685	451	50	1029	DUŻY
10003	Boeing	787-9	16300	315	30	1041	DUŻY
10005	Airbus	A310	9600	262	30	980	DUŻY
10007	Airbus	A340	15900	385	40	980	DUŻY
10006	Airbus	A330	13430	405	40	968	DUŻY
10004	Airbus	A300	7500	266	30	980	DUŻY
10009	Boeing	737-900	6045	215	25	962	ŚREDNI
10010	Boeing	757-300	6287	243	25	980	ŚREDNI
10011	Airbus	A318	5700	132	15	956	ŚREDNI
10012	Airbus	A319	7500	156	16	956	ŚREDNI
10013	Fokker	100	3170	107	10	845	ŚREDNI
10015	Tupolew	TU-204	6500	210	18	858	ŚREDNI
10016	Embraer	190	4260	98	8	870	ŚREDNI
10017	Embraer	195	3334	122	10	882	ŚREDNI
10019	Embraer	ERJ-145 XR	3704	50	4	980	MAŁY
10020	Saab	2000	2185	50	4	662	MAŁY
10021	Antonow	AN-24	2761	50	4	490	MAŁY
10022	Antonow	AN-140	3050	52	4	576	MAŁY
10008	Airbus	A380	15200	660	70	1041	DUŻY
10018	Bombardier	CRJ-100	3710	50	8	956	ŚREDNI
10000	Boeing	747-8	14815	524	60	1047	DUŻY
10014	Embraer	170	3700	70	80	882	ŚREDNI
10023	Boeing	Dreamliner	4000	200	30	1000	DUŻY
\.


--
-- TOC entry 1969 (class 2606 OID 16434)
-- Name: klienci_pkey; Type: CONSTRAINT; Schema: public; Owner: micmax93; Tablespace: 
--

ALTER TABLE ONLY klienci
    ADD CONSTRAINT klienci_pkey PRIMARY KEY (id_klienta);


--
-- TOC entry 1963 (class 2606 OID 16404)
-- Name: lotniska_pkey; Type: CONSTRAINT; Schema: public; Owner: micmax93; Tablespace: 
--

ALTER TABLE ONLY lotniska
    ADD CONSTRAINT lotniska_pkey PRIMARY KEY (id_lotniska);


--
-- TOC entry 1966 (class 2606 OID 16414)
-- Name: loty_pkey; Type: CONSTRAINT; Schema: public; Owner: micmax93; Tablespace: 
--

ALTER TABLE ONLY loty
    ADD CONSTRAINT loty_pkey PRIMARY KEY (id_lotu);


--
-- TOC entry 1972 (class 2606 OID 16439)
-- Name: rezerwacje_pkey; Type: CONSTRAINT; Schema: public; Owner: micmax93; Tablespace: 
--

ALTER TABLE ONLY rezerwacje
    ADD CONSTRAINT rezerwacje_pkey PRIMARY KEY (id_rezerwacji);


--
-- TOC entry 1961 (class 2606 OID 16399)
-- Name: samoloty_pkey; Type: CONSTRAINT; Schema: public; Owner: micmax93; Tablespace: 
--

ALTER TABLE ONLY samoloty
    ADD CONSTRAINT samoloty_pkey PRIMARY KEY (id_samolotu);


--
-- TOC entry 1964 (class 1259 OID 16538)
-- Name: IDX_DATA; Type: INDEX; Schema: public; Owner: micmax93; Tablespace: 
--

CREATE INDEX "IDX_DATA" ON loty USING btree (data_wylotu, id_lotu);


--
-- TOC entry 1967 (class 1259 OID 16537)
-- Name: IDX_NAZWISKO; Type: INDEX; Schema: public; Owner: micmax93; Tablespace: 
--

CREATE INDEX "IDX_NAZWISKO" ON klienci USING btree (nazwisko, id_klienta);


--
-- TOC entry 1970 (class 1259 OID 16539)
-- Name: IDX_REZERWACJE; Type: INDEX; Schema: public; Owner: micmax93; Tablespace: 
--

CREATE INDEX "IDX_REZERWACJE" ON rezerwacje USING btree (id_klienta, id_lotu);


--
-- TOC entry 1973 (class 2606 OID 16415)
-- Name: loty_lotnisko_docelowe_fkey; Type: FK CONSTRAINT; Schema: public; Owner: micmax93
--

ALTER TABLE ONLY loty
    ADD CONSTRAINT loty_lotnisko_docelowe_fkey FOREIGN KEY (lotnisko_docelowe) REFERENCES lotniska(id_lotniska);


--
-- TOC entry 1974 (class 2606 OID 16420)
-- Name: loty_lotnisko_zrodlowe_fkey; Type: FK CONSTRAINT; Schema: public; Owner: micmax93
--

ALTER TABLE ONLY loty
    ADD CONSTRAINT loty_lotnisko_zrodlowe_fkey FOREIGN KEY (lotnisko_zrodlowe) REFERENCES lotniska(id_lotniska);


--
-- TOC entry 1975 (class 2606 OID 16425)
-- Name: loty_samolot_fkey; Type: FK CONSTRAINT; Schema: public; Owner: micmax93
--

ALTER TABLE ONLY loty
    ADD CONSTRAINT loty_samolot_fkey FOREIGN KEY (samolot) REFERENCES samoloty(id_samolotu);


--
-- TOC entry 1977 (class 2606 OID 16544)
-- Name: rezerwacje_id_klienta_fkey; Type: FK CONSTRAINT; Schema: public; Owner: micmax93
--

ALTER TABLE ONLY rezerwacje
    ADD CONSTRAINT rezerwacje_id_klienta_fkey FOREIGN KEY (id_klienta) REFERENCES klienci(id_klienta) ON DELETE CASCADE;


--
-- TOC entry 1976 (class 2606 OID 16532)
-- Name: rezerwacje_id_lotu_fkey; Type: FK CONSTRAINT; Schema: public; Owner: micmax93
--

ALTER TABLE ONLY rezerwacje
    ADD CONSTRAINT rezerwacje_id_lotu_fkey FOREIGN KEY (id_lotu) REFERENCES loty(id_lotu) ON DELETE CASCADE;


--
-- TOC entry 1989 (class 0 OID 0)
-- Dependencies: 5
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2013-03-13 21:16:43

--
-- PostgreSQL database dump complete
--

