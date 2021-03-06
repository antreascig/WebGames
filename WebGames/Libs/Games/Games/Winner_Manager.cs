﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Libs.Games.Games
{
    public class Winner_Manager
    {
        public static bool IsUserWinner(string UserId)
        {
            return Winners.Contains(UserId);
        }
        private static string[] Winners = new string[]
        {
            "22124873-e601-46ea-bba2-3c4ab9e7bb12",
            "31255b8e-6293-44f1-add0-fc2e8b1c4c52",
            "414246f0-3ed4-4a82-b2ed-02ba3e094cc0",
            "6bba13ce-b7c0-49d9-8a2b-c5201962fbd1",
            "6ff2a9ee-43fa-476b-887e-f947670413a0",
            "86125778-b2be-4e55-b118-7bf44ce3fd8e",
            "bf5d1d69-a9d2-40a1-8ea8-21b75f58f03b",
            "e7897781-7484-4332-a850-602a8acade25",
            "faff78d4-b759-410c-803d-fd9130f4cd15",
            "fd26308a-530c-4394-be68-f3c85d12c27a",
            "03a291f8-f5e1-42de-8057-cc0f134cad93",
            "0c550c26-511f-4ff1-a4da-5d166b271467",
            "11d8b28a-60d0-412d-86b8-c3ad6af6883b",
            "14cfb6d6-f367-4eaf-a0a8-1aab81f257b2",
            "34880dac-3b3f-44ce-b289-78693585c451",
            "70fc7e88-9ecc-45d2-96cd-2022c5ec7751",
            "7a716ef4-71ec-4d1a-b390-9196088d41e9",
            "7e4223c6-50f4-4f9f-aa72-93aedcd850b9",
            "8da11d01-3366-4401-9e3b-ed023c2b28f3",
            "98d6c528-fe55-4b34-8593-96976c834782",
            "ae3cd1c8-e08b-487c-976d-08ba14452a75",
            "eed38445-75bb-496c-9f8a-f76975f1b570",
            "4b6128f1-1685-4bf7-88d0-08e8c31595cb",
            "82662fb7-4d29-47de-9965-1f1fd948eca7",
            "8715adb7-53a4-4bd3-b820-2e9854f82238",
            "87914624-7d70-4f23-8488-c121ddde0ef0",
            "adee7b9a-efa3-46c6-ae82-bcb90a49669a",
            "bc4112b3-e2aa-4df8-b1fe-58e1e20d6f75",
            "d6223aba-2f96-45f1-a010-1fe2f804ba5b",
            "d92d99b8-4965-45de-bc8f-4f02d4d20955",
            "fc97ba0c-4eb1-4f3f-a94f-e91c35da3543",
            "0e0482b6-3ab3-4a66-bdc6-584bc27ef164",
            "1be38088-c003-447c-94e4-ca9cb6e18d7f",
            "43491a06-3296-4cb3-ae0c-cdcde80f02bd",
            "664e3c57-d6ab-4179-a329-d38b7c41ce1f",
            "668ec0b1-78cf-4147-8e3a-e7351d665e32",
            "a5db9dac-c79b-4dd1-b0f8-ec45b5496849",
            "d83de976-fe27-4844-81de-b9ad76c2cfdc",
            "d8b51149-98dd-48d9-b6ed-1a3e56d00f61",
            "dc2d2868-1186-4aac-b1b2-5aba61c7b1ed",
            "e103f939-7130-44e4-bc2c-f9bd318e7459",
            "e18491d2-1325-4faf-8afa-02f8fcf3562f",
            "e329ef41-757c-4c63-a04d-8042bf8a8248",
            "1e17aa0d-d59b-49ff-9668-cb35d6a73c60",
            "32a4b25e-b5dd-4d7a-8c2e-a34962572d06",
            "704e7bc7-1ea3-492d-a071-36a4c80a749d",
            "78ce8fa8-03ae-488a-9614-388b64b2cac6",
            "85bbb3b4-7c20-49aa-be91-d5563c26251b",
            "85f5b8ff-16f0-433c-a83a-bf8ad07e234b",
            "a9b114f4-7296-4743-b7cb-55845f94e28f",
            "be8477cb-a1c2-4995-942e-a203c991395b",
            "ddce2f4c-9b72-4ffb-91a5-ec1e675b420b",
            "e69c5d74-7736-417e-9614-f16de4dd373d",
            "33980b9d-00ee-45df-9f43-e52be0035b81",
            "3824c3d2-f914-4a5f-9940-f7010b33699b",
            "3abb09e3-dad2-465b-b6f9-50abdd22f874",
            "5ed929ab-4e9c-4670-b689-3e691751e8b9",
            "75073e0d-8f98-4023-80bc-2d264f74969d",
            "8466f681-bcdd-461a-8bf6-a70971018622",
            "92ff3f9a-0a00-488a-9160-cc70d4e04cec",
            "bdb5ea8a-51d4-41b6-83b2-24dc7739dbf2",
            "c5a258ee-1955-43af-95fb-3beff986065e",
            "ea7101ff-3f66-4b9a-9a71-b9c103701ef6",
            "edfecfd1-22a8-495a-96b2-6b43ed250d17",
            "13dd26db-7620-4e9a-8358-9bc61b561e5e",
            "1ed968ae-1e8f-4281-a714-a30d4e26abdf",
            "3d942bff-7ce0-408b-9530-7b582e21c8ba",
            "48b0302d-90cd-43c5-8791-c494562fb579",
            "48ff70cb-b724-46cd-9442-eed40492eaa0",
            "548e28d1-c42f-469f-a8d0-d301a6ec7c48",
            "58b3fc09-9ea9-42da-938e-e6efb30e97b8",
            "6ef4767f-99e3-4ed5-9e84-f75080868341",
            "731e6a01-514f-4464-a105-3c5d69dfe355",
            "88f3a9f6-160e-409b-bca1-761de9f33910",
            "d53ab362-d01e-4314-a2cc-a9b09d9a3db1",
            "ff9e678a-5c26-43ad-9480-29c8e917310c",
            "0640af3e-1ba7-435d-b198-0a7a739c37e7",
            "1e17ff3f-678d-4491-a6c6-b42b3e84ffcf",
            "21dddddd-2028-413d-870c-26863757f74e",
            "2a885575-9029-467d-8f46-9e7b64cf98ae",
            "37cad24e-00ff-4d32-9aa8-9de86f9a9d7f",
            "538a656c-7614-4f62-8e07-5753444dd94b",
            "6669552f-bf1c-40d7-b363-6e39b088e086",
            "7c426722-ef8d-4809-8701-c88471d95199",
            "930ec733-ebfb-45fe-970e-b2209d04d671",
            "b8bf9527-388e-40fb-a3fc-c5f46f1e7861",
            "c62c3898-0250-42c2-8c28-38f9f9547198",
            "d4401e5c-3e4b-467f-8c2a-825b6aa5fc57",
            "1f9f71bf-284b-401d-851a-68c8b8648f00",
            "20d93fa8-de04-44b8-b966-131dc4cf320b",
            "30aa192f-94cd-44a4-901a-87624e98b64e",
            "45d25a54-df05-43a9-a8e0-cc11af646d78",
            "4f3eee83-ae7f-49c5-8159-443584de96a3",
            "783adff4-aee7-4d7d-a455-d6ac8bb830cf",
            "9590b0f9-4d19-42e8-b127-7de39b9fa3fd",
            "9f6baa58-02a4-401b-8265-843714121df0",
            "e607f396-a258-405e-b6e5-b38cf1f67ed1",
            "f0cf7352-9c0d-4b01-9262-0cd46d6d84f4",
            "f5c9ad99-b444-4de2-9066-38e8c4d66cf3",
            "fce80c0c-8cb4-4108-82d2-09ae4c76dde9",
            "001d4333-97f4-40fb-a5cd-9df6541f14a9",
            "009ca6cc-5b1f-4698-986d-ff8a508f037b",
            "16a1d13e-23e1-483d-abbc-71407596aa41",
            "45bb25ac-bf2a-4f2e-9ab4-b3e579416b9e",
            "4c7f8adb-7afe-4549-83c7-b0d3c67a4b5f",
            "518b411e-0ec8-47fc-8dd9-33b5ce741403",
            "7a67d074-6f2e-4d99-b847-db88fd6483ea",
            "99b339be-9922-4ed9-b82c-2928f3645eb2",
            "9b5ba595-92a9-4d1c-b963-01a45a0eb40a",
            "9bd68a76-0172-4f05-8ecd-3f8be27c07a8",
            "ab753162-3733-44b7-a388-f0c975b70e58",
            "0520dee6-0b3a-40f5-9084-fc84473bc290",
            "0daeeb1e-bba4-4220-905e-3a8c63b2b4f6",
            "27a71194-1f31-400e-8f9a-a9d32cc2080e",
            "2c323e89-d8bd-4670-8b7e-f4f9cfec1aba",
            "697e7cbd-53d7-4146-89ac-b65e48a52d3f",
            "a6f2d5f1-a5ee-4c40-9671-37abead91fc8",
            "ae11636b-6b27-436c-a4b6-513270276a49",
            "bf69690e-a1b8-44c0-b013-aef346b7668e",
            "c3248bf8-078a-4278-ad56-a98b96734c40",
            "f2991c97-e66a-4397-b5ee-2e31e9912b92",
            "fca4d9ce-ae65-47c5-8550-0e621d90f1d6",
            "054cbfe2-7d41-4282-b934-0bc16468f198",
            "2a59ca13-86e8-41ba-ad32-584efac58472",
            "46ed3a77-22db-4e51-b7f2-d37c29ba0344",
            "47bb13ff-f715-4d96-ab29-8afb284a645b",
            "644bfa2d-f346-4a1b-8721-968225e52cf6",
            "6942b939-7648-44e0-a98a-331766f703ff",
            "76cc42a4-52c4-451c-9bbf-5a1d3e376e11",
            "7c44079f-580f-49c8-8948-794b7b6247e0",
            "81b03a4e-670a-41a5-a9e7-4d580e6d1bfd",
            "b58f264a-310b-4d06-8448-811231091de2",
            "c6a21651-cc01-4411-8a9f-28145fc0db11",
            "d15f3b15-1bf0-429a-b008-70a385170882",
            "fd21b712-bca9-445b-9f2a-e2e2d12e3ae8",
            "ba2e6274-2871-4206-ba17-32c1e03f9b9b",
            "8d82c3ad-7f82-4ba3-a76e-b5ac55147a19",
            "20bf58e8-1645-4661-82a3-8d6c869a7a3d",
            "55d898e1-71ab-4c30-abe1-15a904fc916e",
            "8097dd50-8b23-4f1b-828c-5f023ccf83ca",
            "a0d6a2f6-9a6e-48fa-b689-873a962bfdec",
            "f22232de-fe69-49fd-b04b-28eb6401a306",
            "3c3a8e4d-a7cf-44cd-9bfe-5f6bb6a834ff",
            "d77296a5-e74b-4d50-85e9-3027c26c24d3",
            // Test users
            "5c50dd40-383f-42b0-9ea8-0ae317d84b4c", // george@azuredo.com.temp
            "a4f050c0-7a21-4bf5-9ae3-006ec7f96edb", // andreas-cig@hotmail.com
            "79f1c12e-8b81-4f38-bdb4-a6f1af401cc9" // marilena@amuse.gr
        };      
    }
}