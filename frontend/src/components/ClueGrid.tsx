import { GuessResponse } from "../types";

interface Props {
  guesses: GuessResponse[];
}

const LABELS = ["Plays", "Backhand", "Country", "Slams", "High Rank", "Pro Yr", "Titles"];

function ArrowIcon({ direction }: { direction: "up" | "down" | null }) {
  if (!direction) return null;
  return <span className="ml-1 text-xs">{direction === "up" ? "▲" : "▼"}</span>;
}

export default function ClueGrid({ guesses }: Props) {
  if (guesses.length === 0) {
    return (
      <p className="text-[var(--text-muted)] text-sm italic mt-6">
        Make your first guess to see clues appear here.
      </p>
    );
  }

  return (
    <div className="mt-6 overflow-x-auto">
      <div className="min-w-[640px]">
        <div className="grid grid-cols-8 gap-1.5 mb-1.5">
          <div className="text-xs font-semibold text-[var(--text-secondary)] self-end pb-1">Player</div>
          {LABELS.map((l) => (
            <div key={l} className="text-xs font-semibold text-[var(--text-secondary)] text-center self-end pb-1">
              {l}
            </div>
          ))}
        </div>

        {guesses.map((g, idx) => (
          <div key={idx} className="grid grid-cols-8 gap-1.5 mb-1.5">
            <div className="flex items-center px-2 py-2 rounded border border-[var(--border)] bg-[var(--bg-card)] text-[var(--text-primary)] text-sm font-medium truncate">
              {g.guessedPlayerName}
            </div>
            {g.clues.map((c) => (
              <div
                key={c.attributeKey}
                className={`flex items-center justify-center px-1 py-2 rounded text-sm font-semibold ${
                  c.isMatch ? "bg-[var(--accent)] text-[var(--on-accent)]" : "bg-[var(--miss-bg)] text-[var(--text-primary)]"
                }`}
                title={c.label}
              >
                {c.value}
                <ArrowIcon direction={c.direction} />
              </div>
            ))}
          </div>
        ))}
      </div>
    </div>
  );
}