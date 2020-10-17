// ==UserScript==
// @name         Travian - Analytics
// @namespace    http://tampermonkey.net/
// @version      1.0.0
// @description  Análise de adversários.
// @author       Daniel Oliveira
// @match        https://*.travian.com/*
// @require      https://code.jquery.com/jquery-3.4.1.min.js
// @grant        GM_getValue
// @grant        GM_setValue
// @grant        GM.setValue
// @grant        GM.getValue
// @grant        GM_getTab
// @grant        GM_saveTab
// ==/UserScript==

//const REPORT_INTERVAL = 15*60;
//const LOOP_INTERVAL = 60;

// Units in minutes
const REPORT_INTERVAL = 30;
const LOOP_INTERVAL = 2;
const REPORT_URL = "http://domain.com/api/report";


const time = () => {
    return parseInt(new Date().getTime() / 1000);
};

const is_on_ally_members = () => {
    return window.location.pathname === "/allianz.php" && window.location.search === "?action=members";
}

const go_to_ally_members = () => {
    if(is_on_ally_members()) return true;
    if(window.location.pathname !== "/allianz.php"){
        window.location.pathname = "/allianz.php";
        return false;
    }else{
        window.location.search = "?action=members";
        return false;
    }
}

const run = async () => {
    while(!go_to_ally_members()){
        await sleep(1000);
    } // Go to ally page

    var lastRun = await load("last_run");
    if(lastRun === undefined){
        save("last_run", 0);
        lastRun = 0;
    }

    if(lastRun > time() + REPORT_INTERVAL * 60 - LOOP_INTERVAL * 60 * 2){
        console.log("Ignoring. Last report too recent.");
        return;
    }

    var now = new Date();
    if(!(now.getMinutes() % REPORT_INTERVAL * 60 <= LOOP_INTERVAL * 60 * 2.1)){
        console.log("Waiting for the correct time to report.");
        return;
    }

    var forumHref = $("#sidebarBoxAlliance a.forum").attr("href");
    var allyId = parseInt(forumHref.substring(forumHref.lastIndexOf("php?") + 8, forumHref.lastIndexOf("&s=")));

    var json = {
        "alliance":{
            "travianId": allyId,
            "name": $("#details .allianceDetails tbody tr:first-of-type td").text(),
        },
        "players": []
    };
    $(".allianceMembers tbody tr").each(function(index, el){
        $el = $(el);
        let playerName = $el.find("td.player a:first-of-type").text();
        if(playerName === undefined || playerName === ""){
            return;
        }
        let online = $el.find("td.player img:first-of-type").hasClass("online1");
        let id = parseInt($el.find("td.player a:first-of-type").attr("href").replace("/profile/", ""));
        let population = $el.find("td.population").text();
        let villages = $el.find("td.villages").text();
        json.players.push({
            travianId: id,
            name: playerName,
            online: online,
            population: population,
            villages: villages
        });
    });

    var pageUpToDate = await load("page_up_to_date");
    if(!pageUpToDate){
        await save("page_up_to_date", true);
        window.location.reload();
        return;
    }

    // Send report
    console.log(json);
    $.post(REPORT_URL, json, async function(data){
        console.log("Data reported. Response: ");
        console.log(data);
        await save("page_up_to_date", false);
        await save("last_run", time());
    });

};

const save = async (name, value) => {
    if (typeof GM_setValue == "function") {
        GM_setValue(name, value);
    } else {
        await GM.setValue(name, value);
    }
}

const load = async (name) => {
    if (typeof GM_setValue == "function") {
        return GM_getValue(name);
    }
    return await GM.getValue(name);
};


async function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}


(async function () {
    await run();
    setInterval(async function(){
        await run();
    }, LOOP_INTERVAL * 60 * 1000);
})();
