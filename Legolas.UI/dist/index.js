document.getElementById("fetchBtn").addEventListener("click", async () => {
  const resultContainer = document.getElementById("fetchResult");
  resultContainer.innerHTML = "🧙‍♂️ Summoning...";

  try {
    const url = `api/characters`;
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error("Failed to fetch");
    }

    const data = await response.json();

    const container = document.getElementById('characterGrid');

    data.forEach(c => {
      const card = createCharacterCard(c);
      container.appendChild(card);
    });
  } catch (err) {
    resultContainer.innerHTML = "❌ Could not summon data from the realm.";
    console.error(err);
  }
});

document.getElementById("createForm").addEventListener("submit", async (e) => {
  e.preventDefault();

  const createResult = document.getElementById("createResult");
  createResult.innerHTML = "⚒️ Crafting character...";

  const character = {
    name: document.getElementById("name").value,
    race: document.getElementById("race").value,
    age: parseInt(document.getElementById("age").value),
    isGood: document.getElementById("alignment").value === "true",
    appearsInBook: document.getElementById("book").value
  };

  try {
    const url = `api/characters`;
    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(character)
    });

    if (!response.ok) {
      throw new Error("Failed to create character");
    }

    createResult.innerHTML = "✅ Character crafted!";
    document.getElementById("createForm").reset();

    const container = document.getElementById('characterGrid');
    container.insertBefore(createCharacterCard(character), container.firstChild);
  } catch (err) {
    createResult.innerHTML = "❌ Failed to craft character.";
    console.error(err);
  }
});

function createCharacterCard(c) {
  const card = document.createElement('div');
  card.className = 'blob-card';

  card.innerHTML = `
        <div class="blob-content">
          <h2>${c.name}</h2>
          <p><strong>Race:</strong> ${c.race}</p>
          <p><strong>Age:</strong> ${c.age}</p>
          <p><strong>Alignment:</strong> ${c.isGood ? "Good" : "Evil"}</p>
          <p><strong>Book:</strong> ${c.appearsInBook}</p>
        </div>
      `;

  return card;
}
