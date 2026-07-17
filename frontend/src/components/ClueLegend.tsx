interface ClueExample {
  pill: string;
  icon: string;
  isMatch: boolean;
  description: string;
}

const EXAMPLES: ClueExample[] = [
  { pill: "Right", icon: "✓", isMatch: true, description: "Match" },
  { pill: "USA", icon: "✕", isMatch: false, description: "No match" },
  { pill: "8", icon: "▲", isMatch: false, description: "Actual value is higher" },
  { pill: "20", icon: "▼", isMatch: false, description: "Actual value is lower" },
];

export default function ClueLegend() {
  return (
    <ul className="flex flex-col gap-2">
      {EXAMPLES.map((ex) => (
        <li key={ex.description} className="flex items-center gap-3">
          <span
            className={`inline-flex items-center gap-1 px-3 py-1 rounded-full text-sm font-semibold ${
              ex.isMatch ? "bg-[var(--accent)] text-[var(--on-accent)]" : "bg-[var(--miss-bg)] text-[var(--text-primary)]"
            }`}
          >
            {ex.icon === "✓" || ex.icon === "✕" ? `${ex.icon} ${ex.pill}` : `${ex.pill} ${ex.icon}`}
          </span>
          <span className="text-[var(--text-secondary)] text-sm">{ex.description}</span>
        </li>
      ))}
    </ul>
  );
}